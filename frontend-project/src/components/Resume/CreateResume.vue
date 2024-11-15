<template>
    <div class="container-xxl bg-white p-0">

  <NavbarShow />

    <header class="page-header">
      <div class="page-indicator">
        <div
          v-for="(page, index) in nbPages"
          :key="index"
          :class="['page-num', { active: currentPage === index + 1 }]"
        >
          {{ index + 1 }}
        </div>
      </div>
    </header>
    <h1
          class="text-center mb-5 wow fadeInUp"
          data-wow-delay="0.1s"
          style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
        >
           Create your resume
        </h1><hr>
    <div class="form-container">
      <form @submit.prevent="submitCv">
        <!-- Page 1: Contact Infos -->
        <div v-if="currentPage === 1">
          <h1>Contact Infos</h1>
          <input v-model="cv.contact.firstName" placeholder="Prénom" required />
          <input v-model="cv.contact.lastName" placeholder="Nom" required />
          <input v-model="cv.contact.jobTitle" placeholder="Titre" required />
          <input v-model="cv.contact.adress" placeholder="Adresse" required />
          <input v-model="cv.contact.zipCode" placeholder="Code Postal" required />
          <input v-model="cv.contact.ville" placeholder="Ville" required />
          <input v-model="cv.contact.pays" placeholder="Pays" required />
          <input v-model="cv.contact.phone" placeholder="Téléphone" required />
          <input v-model="cv.contact.email" placeholder="Email" required />
          <textarea v-model="cv.contact.bio" placeholder="Bio"></textarea>

          <button type="button" @click="nextPage">Suivant</button>
        </div>
        <!-- Page 2: Education -->
        <div v-if="currentPage === 2">
          <h1>Education</h1>
          <input v-model="cv.education.schoolName" placeholder="Nom de l'école" required />
          <input v-model="cv.education.schoolLocation" placeholder="Lieu de l'école" required />
          <input type="date" v-model="cv.education.startMonth" required />
          <input type="date" v-model="cv.education.endMonth" required />
          <input v-model="cv.education.degree" placeholder="Diplôme" required />
          <input v-model="cv.education.fieldofStudy" placeholder="Domaine d'étude" required />

          <button type="button" @click="prevPage">Précédent</button>
          <button type="button" @click="nextPage">Suivant</button>
        </div>

        <!-- Page 3: Work Experience & Skills -->
        <div v-if="currentPage === 3">
          <h1>Work Experience</h1>
          <input v-model="cv.workExperience.position" placeholder="Poste" required />
          <input
            v-model="cv.workExperience.companyName"
            placeholder="Nom de l'entreprise"
            required
          />
          <input type="date" v-model="cv.workExperience.startMonth" required />
          <input type="date" v-model="cv.workExperience.endMonth" required />

          <h1>Skills</h1>
          <div v-for="(skill, index) in cv.skillsList" :key="index">
            <input v-model="skill.skill" placeholder="Compétence" />
          </div>
          <button type="button" @click="addSkill">Ajouter une compétence</button>

          <button type="button" @click="prevPage">Précédent</button>
          <button type="submit">Terminer</button>
        </div>
      </form>
    </div>
   <hr> <h1>Preview</h1><hr>
    <div id="cvContent">
      <div id="profile">
        <p id="name">
          {{ cv.contact.firstName }} {{ cv.contact.lastName }} <br /><span id="email">{{
            cv.contact.email
          }}</span>
        </p>
        <p id="designation">
          {{ cv.contact.jobTitle }} <br /><span id="college">{{ cv.education.schoolName }}</span>
        </p>
        <div id="social-links">
          <a href="#"><i class="fab fa-facebook-f stroke-transparent"></i></a
          ><a><i class="fab fa-twitter stroke-transparent"></i></a
          ><a><i class="fab fa-linkedin-in stroke-transparent"></i></a
          ><a><i class="fab fa-github stroke-transparent"></i></a>
        </div>
        <hr width="100%" />
        <div id="about">
          <p style="display: inline">Adress</p>
        </div>
        <p id="year-graduation">{{ cv.contact.adress }}<br /></p>
        <p id="more-about">
          <span>{{ cv.contact.zipCode }},{{ cv.contact.ville }}, {{ cv.contact.pays }} </span>
        </p>
        <p id="telephone">
          Telephone<br /><strong>{{ cv.contact.phone }}</strong>
        </p>
      </div>
      <div id="info-cards">
        <div class="card">
          <p><i class="fas fa-briefcase stroke-transparent"></i>&nbsp;&nbsp;&nbsp;Profile</p>
          <a href="#">{{ cv.contact.bio }}</a>
        </div>
        <!-- Expérience professionnelle -->
        <div class="card">
          <p>
            <i class="fas fa-briefcase stroke-transparent"></i>&nbsp;&nbsp;&nbsp;Expérience
            professionnelle
          </p>
          <p>
            {{ cv.workExperience.position }} chez {{ cv.workExperience.companyName }}<br />
            {{ cv.workExperience.startMonth }} - {{ cv.workExperience.endMonth }}
          </p>
        </div>
        <div class="card">
          <p><i class="fas fa-graduation-cap stroke-transparent"></i>&nbsp;&nbsp;&nbsp;Education</p>
          <ul>
            <li>
              <p class="tags">
                {{ cv.education.schoolName }}<br /><span>
                  {{ cv.education.degree }} on {{ cv.education.fieldofStudy }} |
                  <span>{{ cv.education.schoolLocation }}</span></span
                >
              </p>
              <a class="edit" href="#"><i class="fas fa-pen stroke-transparent-blue"></i></a>
            </li>
          </ul>
        </div>
        <!-- Compétences -->
        <div class="card">
          <p><i class="fas fa-lightbulb stroke-transparent"></i>&nbsp;&nbsp;&nbsp;Compétences</p>
          <ul>
            <li v-for="(skill, index) in cv.skillsList" :key="index">{{ skill.skill }}</li>
          </ul>
        </div>
      </div>
    </div>
    <button @click="generatePdf">Générer le PDF</button>
  <FooterShow />
  </div>
</template>

<script>
import FooterShow from '../footer/FooterShow.vue'
import NavbarShow from '../navbar/NavbarShow.vue'
import { ref, reactive } from 'vue'
import html2pdf from 'html2pdf.js'

export default {
  name: 'CreateResume',
  components: {
    NavbarShow,
    FooterShow
  },
  setup() {
    const currentPage = ref(1)
    const nbPages = 3

    // État réactif pour le CV
    const cv = reactive({
      contact: {
        firstName: '',
        lastName: '',
        jobTitle: '',
        adress: '',
        zipCode: '',
        ville: '',
        pays: '',
        phone: '',
        email: '',
        bio: ''
      },
      education: {
        schoolName: '',
        schoolLocation: '',
        startMonth: '',
        endMonth: '',
        degree: '',
        fieldofStudy: ''
      },
      workExperience: {
        position: '',
        companyName: '',
        startMonth: '',
        endMonth: ''
      },
      skillsList: [{ skill: '' }]
    })

    // Méthodes de navigation entre les pages
    const nextPage = () => {
      if (currentPage.value < nbPages) currentPage.value++
    }

    const prevPage = () => {
      if (currentPage.value > 1) currentPage.value--
    }

    // Méthode pour ajouter une compétence
    const addSkill = () => {
      cv.skillsList.push({ skill: '' })
    }

    // Méthode pour soumettre le formulaire
    const submitCv = async () => {
      try {
        const response = await fetch('http://localhost:5094/api/CV', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(cv)
        })

        if (!response.ok) {
          throw new Error('Erreur lors de la soumission du CV')
        }

        const data = await response.json()
        console.log('CV créé avec succès:', data)
      } catch (error) {
        console.error('Erreur:', error)
      }
    }

    // Méthode pour générer le PDF
    const generatePdf = () => {
      const element = document.getElementById('cvContent')
      setTimeout(() => {
    const options = {
      margin: 10,
      filename: 'cv.pdf',
      html2canvas: { scale: 2, useCORS: true },
      jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };

    html2pdf()
      .from(element)
      .set(options)
      .save('cv.pdf')
      .then(() => {
        element.classList.remove('pdf-layout'); // Supprime les styles PDF après la génération
      });
  }, 100); 
    }

    return {
      currentPage,
      nbPages,
      cv,
      generatePdf,
      nextPage,
      prevPage,
      addSkill,
      submitCv
    }
  }
}
</script>
<style scoped>
.page-header {
  text-align: center;
  margin-bottom: 20px;
}

.page-indicator {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin-bottom: 10px;
}

.page-num {
  width: 30px;
  height: 30px;
  line-height: 30px;
  border-radius: 50%;
  background-color: lightgray;
  color: #333;
  display: inline-block;
  text-align: center;
  font-weight: bold;
  transition:
    background-color 0.3s ease,
    color 0.3s ease;
}

.page-num.active {
  background-color: #4caf50;
  color: white;
}

.page-num:hover {
  cursor: pointer;
  background-color: #45a049;
  color: white;
  transform: scale(1.1);
}

.active {
  color: white;
  background-color: blue;
}

.form-container {
  display: flex;
  justify-content: center; /* Centre horizontalement */
  align-items: center; /* Centre verticalement */
  min-height: 50vh; /* Prend toute la hauteur de la page */
  padding: 10px; /* Ajoute un peu de padding */
}

form {
  width: 100%;
  max-width: 600px;
  background-color: #f9f9f9;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

input,
textarea {
  width: 100%; /* Utiliser 100% pour une largeur constante */
  max-width: 600px; /* Fixer la largeur maximale à 600px */
  margin: 10px 0; /* Marges uniformes */
  padding: 10px;
  border: 1px solid lightgrey;
  border-radius: 5px;
}

button {
  background-color: #4caf50;
  color: white;
  cursor: pointer;
}

button:hover {
  background-color: #45a049;
}

html {
  font-family: 'Open Sans', sans-serif;
  background: whitesmoke;
}

a {
  text-decoration: none;
  color: black;
}

hr {
  background: grey;
  margin: 20px 0;
}

#cvContent {
  display: flex;
  margin: 0 auto;
  max-width: 900px;
}

#profile {
  flex: 15%;
  margin: 5% 2% 0 10%;
  width: 45%;
}

#profile-container {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 20px;
}

.pdf-layout #cvContent {
  max-width: 100px;
}

.pdf-layout #profile,
.pdf-layout #info-cards {
  width: 100%;
  margin-top: 5%;
}

#address {
  width: 45%; /* Ajuste la largeur pour l'adresse */
}

#info-cards {
  flex: 55%;
  margin-top: 5%;
  width: 100%;
}

#image,
#profile-photo {
  width: 80px;
  height: 80px;
  border-radius: 10px;
}

#image {
  position: relative;
  overflow: hidden;
}

#image > a {
  position: absolute;
  top: 0;
  left: 0;
  background: rgba(0, 0, 0, 0.5);
  height: 100%;
  width: 100%;
  display: none;
}

#image:hover a {
  display: block;
}

#name {
  font-size: 23px;
  line-height: 20px;
}

#about,
.card > ul > li {
  padding-left: 15px;
  width: 100%;
}

#about {
  font-size: 20px;
  padding: 0;
}

#name,
#about > p {
  font-weight: bold; /* Utilisation de 'bold' au lieu de 'bolder' */
}

#email {
  font-size: 15px;
  font-weight: bold;
  font-family: 'Cutive Mono', monospace;
}

#college,
#email,
#year-graduation,
#education,
#more-about,
#telephone,
#fax {
  color: #555;
  font-size: 13.5px;
}

strong,
span {
  color: black;
  font-size: 16px;
}

#social-links,
#about {
  display: inline-block;
}

#social-links {
  margin-bottom: 12px;
}

#social-links a {
  margin: 0 10px;
}

#edit-intro {
  display: block;
  color: #097bbf;
  font-family: 'Nunito', sans-serif;
}

.fab,
.fas {
  color: whitesmoke;
}

#about > a,
.edit {
  position: absolute;
  font-size: 15px;
}

.stroke-transparent {
  -webkit-text-stroke: 1px #000;
  -webkit-text-fill-color: transparent;
}

.blue {
  color: #097bbf;
  font-size: 13px;
}

.stroke-transparent-blue {
  -webkit-text-stroke: 1px #097bbf;
  -webkit-text-fill-color: transparent;
}

.card {
  box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
  overflow-x: hidden;
  margin-bottom: 30px;
  padding: 15px 30px 30px 30px;
  background-color: #fff;
}

.card > p {
  color: #0e141e;
  font-weight: bold;
  font-size: 18px;
  line-height: 2;
}

.card > a {
  font-weight: 400;
  font-size: 15px;
  margin-left: 25px;
  padding: 0;
  border: 0;
  background: transparent;
  color: #097bbf;
  outline: none;
  cursor: pointer;
}

.card > ul {
  list-style-type: none;
}

.tags {
  font-size: 17px;
  font-weight: bold;
}

.tags span {
  font-size: 14px;
  font-weight: normal;
  color: #0e141e;
}

.tags span span {
  color: #738f93;
}

@media screen and (max-width: 1090px) {
  #profile {
    margin-left: 5%;
  }
}

@media screen and (max-width: 850px) {
  #cvContent {
    display: block;
  }

  #profile {
    width: 90%;
  }

  .card {
    margin: 0 5%;
    margin-bottom: 30px;
  }
}
</style>
