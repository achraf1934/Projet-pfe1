from flask import Flask, request, jsonify
import docx
from PyPDF2 import PdfReader
import os
import shutil
from werkzeug.utils import secure_filename
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import string
import pytesseract
from pdf2image import convert_from_path
from PIL import Image

app = Flask(__name__)

# Chemin temporaire pour les fichiers uploadés
UPLOAD_FOLDER = 'uploads'
if not os.path.exists(UPLOAD_FOLDER):
    os.makedirs(UPLOAD_FOLDER)

# Fonction pour extraire le texte d'un fichier DOCX
def docx_to_string(docx_path):
    doc = docx.Document(docx_path)
    return ' '.join([para.text for para in doc.paragraphs])

# Fonction pour extraire le texte d'un fichier PDF
def extract_text_from_pdf(pdf_path):
    with open(pdf_path, 'rb') as file:
        reader = PdfReader(file)
        return ' '.join([page.extract_text() or '' for page in reader.pages])

# Fonction de secours : Extraction de texte par OCR à partir d'un PDF
def extract_text_from_pdf_with_ocr(pdf_path):
    images = convert_from_path(pdf_path)
    text = ""
    for image in images:
        text += pytesseract.image_to_string(image)
    return text

# Prétraitement du texte pour la similarité
def preprocess_text(text):
    text = text.lower().translate(str.maketrans('', '', string.punctuation))
    return text

# Calcul du score de matching exact des compétences
def calculate_skill_match(resume_text, required_skills):
    resume_words = set(resume_text.lower().split())
    required_skills = {skill.strip().lower() for skill in required_skills.split(',')}
    match_count = sum(1 for skill in required_skills if skill in resume_words)
    return (match_count / len(required_skills)) * 100  # Score en %

# Calcul de la similarité cosinus entre CV et description de poste
def rank_resume(resume, job_description):
    resume = preprocess_text(resume)
    job_description = preprocess_text(job_description)
    vectorizer = TfidfVectorizer(stop_words='english')
    tfidf_matrix = vectorizer.fit_transform([resume, job_description])
    similarity = cosine_similarity(tfidf_matrix[0:1], tfidf_matrix[1:2])
    return similarity[0][0] * 100
def extract_relevant_text(resume_text):
    # Filtrer uniquement les sections "Skills" ou "Compétences"
    keywords = ["skills", "competences", "technologies"]
    lines = resume_text.splitlines()
    relevant_text = " ".join([line for line in lines if any(kw in line.lower() for kw in keywords)])
    return relevant_text if relevant_text else resume_text
def calculate_final_score(similarity_score, skill_match_score):
    # Normaliser le score entre 0 et 10
    final_score = ((similarity_score * 0.5) + (skill_match_score * 0.5)) / 10 * 10
    return min(10, max(0, final_score))
@app.route('/compare', methods=['POST'])
def compare_resume():
    try:
        required_skills = request.form.get('required_skills')
        cv_file = request.files['cv']

        if not required_skills or not cv_file:
            return jsonify({'error': 'Missing required fields'}), 400

        # Enregistrer temporairement le fichier CV
        filename = secure_filename(cv_file.filename)
        filepath = os.path.join(UPLOAD_FOLDER, filename)
        cv_file.save(filepath)

        # Déterminer le type de fichier et extraire le texte
        if filepath.endswith('.pdf'):
            resume_text = extract_text_from_pdf(filepath)
            if not resume_text.strip():
                resume_text = extract_text_from_pdf_with_ocr(filepath)  # OCR en dernier recours
        elif filepath.endswith('.docx'):
            resume_text = docx_to_string(filepath)
        else:
            return jsonify({'error': 'Unsupported file type'}), 400
 # Extraire uniquement le texte pertinent pour comparaison
        relevant_resume_text = extract_relevant_text(resume_text)

        skill_match_score = calculate_skill_match(relevant_resume_text, required_skills)


        # Log des scores pour debugging
        print(f"Score de matching des compétences: {skill_match_score:.2f}%")


        # Nettoyage du fichier temporaire
        os.remove(filepath)

        return str(skill_match_score)
    except Exception as e:
        print(f"Erreur: {e}")
        return jsonify({'error': 'An error occurred during processing'}), 500

if __name__ == '__main__':
    app.run(debug=True)
