using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace premierTest
{
    public partial class Mutuelle
    {
        HillthDataClassDataContext HDB = new HillthDataClassDataContext();
        private int idmutuelle;
        private string libelMutuelle;

        public int IdHopital
        {
            get { return idmutuelle; }
            set { idmutuelle = value; }
        }

        public string LibHopital
        {
            get { return libelMutuelle; }
            set { libelMutuelle = value; }
        }

        // Constructeur
        public Mutuelle(int id, string lib)
        {
            idmutuelle = id;
            libelMutuelle = lib;
        }

        // Ressort l'ID de la mutuelle entrée en parametre
        public static int AfficherLibelMut(string libelMut)
        {
            HillthDataClassDataContext HDB = new HillthDataClassDataContext();
            int mutuelle = 0;
            var queryMut = from m in HDB.Mutuelle where m.LibelMutuelle == libelMut select m.IDMutuelle;
            foreach (int m in queryMut)
            {
                mutuelle = m;
            }
            return mutuelle;
        }

        // Ajoute toutes les mutuelles de la base de données dans une list
        public List<string> DisplayMutuelle()
        {
            List<string> listMutuelle = new List<string>();
            var query2 = from m in HDB.Mutuelle select m.LibelMutuelle;
            // Mutuelle
            foreach (string m in query2)
            {
                listMutuelle.Add(m);
            }
            return listMutuelle;
        }

        // Liste comptant le nombre d'utilisateurs pour chaque mutuelles
        public List<string> CptMutuelle()
        {
            List<string> listMut = new List<string>();
            int queryCountMut = (from m in HDB.Mutuelle select m.IDMutuelle).Count();

            for (int i = 0; i < queryCountMut; i++)
            {
                var queryCountElemMut = (from p in HDB.FichePatient where p.IDMutuelle == i+1 select p).Count();
                var nomMut = (from m in HDB.Mutuelle select m.LibelMutuelle).ToArray();

                listMut.Add(queryCountElemMut + " personnes sont affiliées à  " + nomMut[i] + ".\n");
            }
            return listMut;
        }
    }
}
