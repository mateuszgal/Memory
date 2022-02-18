using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    internal class Result
    {

        private string nameOfplayer;
        private DateOnly dateOfresult;
        private double time;
        private int tries;
        private string level;
        public Result(string name, DateOnly date, double guessingtime, int guessingTries, string level)
        {
            nameOfplayer = name;
            dateOfresult = date;
            time = guessingtime;
            tries = guessingTries;
            this.level = level;
        }
        public Result(string[] table)
        {
            nameOfplayer = table[0];
            dateOfresult = DateOnly.Parse(table[1]);
            time = double.Parse(table[2]);
            tries = int.Parse(table[3]);
            level = table[4];
        }
        public string NameOfplayer
        {
            get { return nameOfplayer; }
        }
        public DateOnly DateOfresult
        {
            get { return dateOfresult; }
        }
        public double Time
        {
            get { return time; }
        }
        public int Tries
        {
            get { return tries; }
        }
        public string Level
        {
            get { return level; }
        }

    }
}
