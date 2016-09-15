using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace premierTest
{
    public partial class Hopital
    {
        private int idhopital;
        private string libelHopital;

        public int IdHopital
        {
            get { return idhopital; }
            set { idhopital = value; }
        }

        public string LibHopital
        {
            get { return libelHopital; }
            set { libelHopital = value; }
        }

        // Constructeur
        public Hopital  (int id, string lib)
        {
            idhopital = id;
            libelHopital = lib;
        }

        // Affiche le numero de l'hopital entré en paramètre
        public static int AfficherLibelHop(string libelHop)
        {
            HillthDataClassDataContext HDB = new HillthDataClassDataContext();
            HDB.AfficherLibelHop();
            int hopital = 0;
            // Hôpital
            var queryHop = from h in HDB.Hopital where h.LibelHopital == libelHop select h.IDHopital;
            foreach (int h in queryHop)
            {
                hopital = h;
            }
            return hopital;
        }

        // Affiche les hopitaux dans le combobox pour ajouter un patient, les hopitaux sont renvoyés via une liste
        public List<string> DisplayHopital()
        {
            HillthDataClassDataContext HDB = new HillthDataClassDataContext();
            List<string> listHopitaux = new List<string>();
            var query1 = from h in HDB.Hopital select h.LibelHopital;
            foreach (string h in query1)
            {
                listHopitaux.Add(h);
            }
            return listHopitaux;
        }
    }
}
