using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace premierTest
{
    /* CLASSE ABSTRAITE */
    partial class Medecin
    {
        //Appel de la dataclass (base de données)
        HillthDataClassDataContext HDB = new HillthDataClassDataContext();
        private string nomMed;
        private string preMed;
        private DateTime dateNaissanceMed;
        private string sexeMed;
        private string idMedecin;
        private string lieuTravail;
        private string numTelMed;
        private int idspec;
        private string conventionMed;

        public string NomMed
        {
            get { return nomMed; }
            set { nomMed = value; }
        }

        public string PreMed
        {
            get { return preMed; }
            set { preMed = value; }
        }

        public DateTime DateNaissanceMed
        {
            get { return dateNaissanceMed; }
            set { dateNaissanceMed = value; }
        }

        public string SexeMed
        {
            get { return sexeMed; }
            set { sexeMed = value; }
        }

        public string IdMedecin
        {
            get { return idMedecin; }
            set { idMedecin = value; }
        }

        public string LIeuTravail
        {
            get { return lieuTravail; }
            set { lieuTravail = value; }
        }

        public string NumTelMed
        {
            get { return numTelMed; }
            set { numTelMed = value; }
        }

        public int IdSpec
        {
            get { return idspec; }
            set { idspec = value; }
        }

        public string ConventionMed
        {
            get { return conventionMed; }
            set { conventionMed = value; }
        }

        // Constructeur
        public Medecin(string nom, string pre, DateTime dateNaiss, string sexe, string idMed, string lieuTrav,
            string numTel, int IDSpec, string convention)
        {
            nomMed              = nom;
            preMed              = pre;
            dateNaissanceMed    = dateNaiss;
            sexeMed             = sexe;
            idMedecin           = idMed;
            lieuTravail         = lieuTrav;
            numTelMed           = numTel;
            idspec              = IDSpec;
            conventionMed       = convention;
        }

        // Fonction permettant de créer un médecin de n'importe quel spécialité
        public void CreerMedecin()
        {
            try
            {
                HDB.InsertMedecin(nomMed, preMed, numTelMed, sexeMed, lieuTravail, dateNaissanceMed, idMedecin, conventionMed, idspec + 1);
                // Insere dans la DB
                HDB.SubmitChanges();
                MessageBox.Show("Enregistrement effectué !");
            }
            catch (Exception e)
            {
                MessageBox.Show("Une erreur est survenue.");
                MessageBox.Show(e.Message);
            }
        }

        // Affiche le profil du médecin si profil il y a.
        public void AfficherProfil(string _USRID, out string txt_Nom, out string txt_Spec,
            out string txt_numTel, out string txt_INAMI, out string txt_Adresse)
        {
            // Les valeurs sont initialisées quoi qu'il arrive.
            txt_Nom = txt_Spec = txt_numTel = txt_INAMI = txt_Adresse = "";
            var query = from m in HDB.Medecin
                        where m.IDMedecin == _USRID
                        select m;
            // S'il y au un résultat, affiche les informations suivantes : 
            if (query.Any())
            {
                foreach (var q in query)
                {
                    txt_Nom     = "Docteur "        + q.Nom + " " + q.Prenom;
                    txt_Spec    = "Conventionné : " + q.Convention;
                    txt_numTel  = "Téléphone    : " + q.NumTel.ToString();
                    txt_INAMI   = "Numero INAMI : " + q.IDMedecin;
                    txt_Adresse = "Adresse : "      + q.LieuTravail;
                }
            }
            else
            {
                MessageBox.Show("Aucun médecin ne correspond à votre n° INAMI. \n -> " + _USRID);
            }
        }

        // Compte le nombre de médecins présent dans la base de données (pour statistique)
        public string CptMedecin()
        {
            return (from m in HDB.Medecin select m.IDMedecin).Count().ToString();
        }

        // Utilisé pour ajouter le médecin d'un patient dans la table HistoriquePatient
        public string retourneNumINAMI (string nomMed)
        {
            // Par défut l'ID vaut : "00...00" étant donné qu'il faut de toute façon retourner une valeur.
            string idMed = "000000000000";
            var rechercheINAMI = from i in HDB.Medecin where i.Nom == nomMed select i;
            // Si une correspondance est trouvée, on retourne l'ID du médecin
            if (rechercheINAMI.Any())
            {
                foreach(var id in rechercheINAMI)
                {
                    idMed = id.IDMedecin;
                }
            }
            return idMed;
        }

        // Affiche le nom du médecin dans l'entête
        public void AfficherNomMed(Accueil A)
        {
            var query = from m in HDB.Medecin
                        where m.IDMedecin == A._USRID
                        select m;
            if (query.Any())
            {
                foreach (var q in query)
                {
                    A.txt_username.Text = "Vous êtes connecté sous le nom [ " + q.Nom + " " + q.Prenom + " ]";
                }
            }
            else
            {
                // Si la personne n'a pas de profil créé, on lui affiche juste son numero INAMI
                A.txt_username.Text = "ID : [" + A._USRID + "]";
            }
        }

        // Savoir si on peut ajouter un nouveau patient ou non, car s'il n'y a pas de médecin généraliste dans la base de donnée, ça ne va pas pour la suite
        public bool MedGedDejaCree()
        {
            bool existe = false;
            var query = from m in HDB.Medecin
                        // 1 représente un généraliste
                        where m.IDSpec == 1
                        select m;
            if (query.Any())
            {
                existe = true;
            }
            return existe;
        }

        // Liste contenant le nom des médecins généraliste uniquement
        public List<string> DisplayMedecinGeneraliste()
        {
            List<string> listMed = new List<string>();
            var query3 = from med in HDB.Medecin where med.IDSpec == 1 select med.Nom;

            // Nom du médecin généraliste s'il y en a un sinon renvoie "Aucun"
            if (query3.Any())
            {
                foreach (string med in query3)
                {
                    // On ajoute le médecin dans la list s'il en existe
                    listMed.Add(med);
                }
            }
            else
            {
                listMed.Add("Aucun");
            }
            return listMed;
        }
    }
}