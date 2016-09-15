using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace premierTest
{
    public partial class HistoriquePatient
    {
        private string idmedecin;
        private string idpatient;
        private DateTime dateauscultation;

        public string IDmedecin
        {
            get { return idmedecin; }
            set { idmedecin = value; }
        }

        public string IDpatient
        {
            get { return idpatient; }
            set { idpatient = value; }
        }

        public DateTime Dateauscultation
        {
            get { return dateauscultation; }
            set { dateauscultation = value; }
        }

        // Constructeur
        public HistoriquePatient(string idmed, string idpat, DateTime today)
        {
            idmedecin = idmed;
            idpatient = idpat;
            dateauscultation = today;
        }

        // Met à jour ces valeurs quand une nouvelle personne est créée
        public void AjoutHistorique()
        {
            try
            {
                using (HillthDataClassDataContext HDB = new HillthDataClassDataContext())
                {
                    // Insertion dans la DC
                    HDB.InsertHistorique(idmedecin, dateauscultation, idpatient);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nErreur dans AjoutHistorique.");
            }
        }
    }
}
