using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeCrew.Torpedo
{
    class ComputerPlayer
    {
        const int NO_HIT_BEFORE = 0;
        const int ONE_HIT_BEFORE = 1;
        const int TWO_HIT_BEFORE = 2;

        const int DIR_UP = 1;
        const int DIR_LEFT = 2;
        const int DIR_DOWN = 3;
        const int DIR_RIGHT = 4;

        int width, height;

        List<List<int>> used;

        int stage;
        List<int[]> LastTwoPoints;

        private Random calculator;

        public ComputerPlayer(int width, int height)
        {
            this.width = width;
            this.height = height;

            used = new List<List<int>>();

            stage = 0;
            LastTwoPoints = new List<int[]>();

            used = new List<List<int>>();
            for(int i=0;i< height; i++)
            {
                used.Add(new List<int>());
                for(int j=0; j< width;j++)
                {
                    used.Last().Add(j);
                }
            }
            
            calculator = new Random();
        }

        public int[] calculateNextHit()
        {
            int[] hit;
            switch(stage)
            {
                case NO_HIT_BEFORE:
                    hit = calculateBaseStep();
                    break;

                case ONE_HIT_BEFORE:
                    hit = calculateOneHitStep();
                    break;

                default: hit = calculateMultipleHitSetp();
                    break;
            }

            used[hit[0]].Remove(hit[1]);

            return hit;
        }

        public void setHit(int x, int y)
        {
            LastTwoPoints.Add(new int[] { x, y });
            if(LastTwoPoints.Count > 2)
            {
               int[] cords = new int[LastTwoPoints.Count];
               if(LastTwoPoints.First()[0] == LastTwoPoints.Last()[0])
               {
                    cords = new int[] { LastTwoPoints[0][0], LastTwoPoints[0][1], LastTwoPoints[0][2] };
                    Array.Sort(cords);

                    int j = 0;
                    while(j < LastTwoPoints.Count)
                    { 
                        if (LastTwoPoints[j][1] != cords[0] || LastTwoPoints[j][1] != cords[cords.Length-1])
                        {
                            LastTwoPoints.RemoveAt(j);
                        }
                        else { j++; }
                        
                    }
               }
               else
               {
                    cords = new int[] { LastTwoPoints[0][0], LastTwoPoints[1][0], LastTwoPoints[2][0] };
                    Array.Sort(cords);

                    int j = 0;
                    while (j < LastTwoPoints.Count)
                    {
                        if (LastTwoPoints[j][0] != cords[0] || LastTwoPoints[j][0] != cords[cords.Length - 1])
                        {
                            LastTwoPoints.RemoveAt(j);
                        }
                        else { j++; }
                    }
                }
            }

            if (stage < 3)
                stage++;
        }

        public void setShipExplosion()
        {
            LastTwoPoints.Clear();
            stage = 1;
        }

        private int[] calculateBaseStep()
        {
            List<int> usable_rows = new List<int>();
            for(int i=0;i<used.Count;i++)
            {
                if (used[i].Count != 0)
                    usable_rows.Add(i);
            }

            int row = usable_rows[calculator.Next(0, usable_rows.Count)];

            int col = used[row][calculator.Next(0, used[row].Count)];

            return new int[] { row, col };
        }

        private int[] calculateOneHitStep()
        {
            int prev_X = LastTwoPoints.Last()[0];
            int prev_Y = LastTwoPoints.Last()[1];

            List<int> directions = new List<int>();
            if (prev_X != 0 && used[prev_X - 1].Contains(prev_Y))
            {
                directions.Add(DIR_LEFT);
            }

            if (prev_X < height - 1 && used[prev_X + 1].Contains(prev_Y))
            {
                directions.Add(DIR_RIGHT);
            }

            if (prev_Y != 0 && used[prev_X].Contains(prev_Y - 1))
            {
                directions.Add(DIR_UP);
            }

            if (prev_Y < width && used[prev_X].Contains(prev_Y + 1))
            {
                directions.Add(DIR_DOWN);
            }

            int dir = directions[calculator.Next(0, directions.Count)];

            switch (dir)
            {
                case DIR_UP:
                    return new int[] { prev_X - 1, prev_Y };

                case DIR_LEFT:
                    return new int[] { prev_X, prev_Y - 1};

                case DIR_DOWN:
                    return new int[] { prev_X + 1, prev_Y };

                case DIR_RIGHT:
                    return new int[] { prev_X, prev_Y + 1 };

                default: return calculateBaseStep();
            }
        }

        private int[] calculateMultipleHitSetp()
        {
            List<int> dir = new List<int>();

            int[] cols;

            if (LastTwoPoints.First()[0] == LastTwoPoints.Last()[0])
            {
                cols = new int[] { LastTwoPoints.First()[1], LastTwoPoints.Last()[1] };
            }
            else
            {
                cols = new int[] { LastTwoPoints.First()[0], LastTwoPoints.Last()[0] };
            }

            Array.Sort(cols);

            if (cols[0] > 0)
                dir.Add(cols[0] - 1);

            if (cols[1] < width - 1)
                dir.Add(cols[1] + 1);

            if (LastTwoPoints.First()[0] == LastTwoPoints.Last()[0])
                return new int[] { LastTwoPoints.First()[0], dir[calculator.Next(0, dir.Count)] };
            else
                return new int[] { dir[calculator.Next(0, dir.Count)], LastTwoPoints.First()[1] };
        }
    }
}
