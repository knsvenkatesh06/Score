using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Score
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalScore = 0, wickets = 0, finishOvers = 0, extraRuns = 0;
            int batsman1 = 0, batsman2 = 1;
            bool BowlersP = true;           
            int batsman1Runs = 0, batsman2Runes = 0;
            int batsmanstrick = batsman1;
            int balls = 0;
            string[][] arr = new string[5][];
            arr[0] = new string[] { "2", "0", "2lb", "1", "0", "1wd", "4", "0", "0", "4", "0wd", "4wd", "3", "4" };
            arr[1] = new string[] { "4", "6nb", "3", "WN", "0", "1", "1lb", "0nb", "2", "1", "1wd", "6", "1", "2", "3lb" };
            arr[2] = new string[] { "1", "4", "W", "0", "6", "2", "4nb", "0", "3", "1", "2wd", "4", "2", "2" };
            arr[3] = new string[] { "1", "0", "1", "1", "4", "2b", "W", "0", "1", "6wd", "4", "1", "1nb", "1b" };
            arr[4] = new string[] { "0", "2", "1WN", "4", "2", "3", "6", "2", "1", "0", "1wd", "6", "0wd", "1" };
            var getrules = Teams.GetRules();
            var team1 = Teams.GetTeamIndia();
            var team2 = Teams.GetTeamAustralia();


            for (int i = 0; i < arr.Length; i++)
            {

                foreach (var ball in arr[i])
                {
                    balls = balls + 1;
                    int run = 0;
                    bool batsmanNeedChange = true;
                    bool batsmanrun = true;
                    var strRuns = new string(ball.Where(x => Char.IsDigit(x)).ToArray());
                    int runs = 0;
                    if (!string.IsNullOrEmpty(strRuns))
                        runs = Convert.ToInt32(strRuns);
                    var extras = ball.Where(x => !Char.IsDigit(x));
                    if (extras.Any())
                    {
                        string extra = new string(extras.ToArray());
                        if (!string.IsNullOrEmpty(extra))
                        {
                            var rule = getrules.Where(x => x.Rule.ToLower() == extra.ToLower()).FirstOrDefault();
                            if (rule != null)
                            {
                                if (rule.ExtraRun > 0)
                                {
                                    extraRuns = extraRuns + rule.ExtraRun;
                                    run = rule.ExtraRun;
                                    team2[BowlersP ? 0 : 1].Extras = team2[BowlersP ? 0 : 1].Extras + 1;
                                    balls = balls - 1;
                                    if (balls == 0)
                                        batsmanNeedChange = false;

                                }
                                if (!rule.BatsmanRun)
                                {
                                    batsmanrun = false;
                                    extraRuns = extraRuns + runs;
                                }
                                if (rule.BatsmanOut)
                                {

                                    wickets = wickets + 1;
                                    team1[batsman1].BatsmanOut = true;
                               
                                    var battingOrder = batsman1 > batsman2 ? batsman1 + 1 : batsman2 + 1;
                                    batsman1 = rule.NextBatsManFacingNewBall ? battingOrder : batsman2;
                                    batsman2 = rule.NextBatsManFacingNewBall ? batsman2 : battingOrder;
                                    batsmanstrick = batsman1;
                                }

                            }

                            run = run + runs;
                        }


                    }
                    else
                    {
                        run = runs;

                    }
                  var strickChange = (runs % 2 != 0);                  
                    if (batsmanrun)
                    {
                        if (batsman1 == batsmanstrick)
                            batsman1Runs = batsman1Runs + runs;
                        else
                            batsman2Runes = batsman2Runes + runs;
                    }
                    if (batsmanrun && runs > 0)
                    {
                        team1[batsmanstrick == batsman1 ? batsman1 : batsman2].Score = team1[batsmanstrick == batsman1 ? batsman1 : batsman2].Score + runs;

                        if (runs == 4)
                            team1[batsmanstrick == batsman1 ? batsman1 : batsman2].Fours = team1[batsmanstrick == batsman1 ? batsman1 : batsman2].Fours + 1;
                       else if (runs == 6)
                            team1[batsmanstrick == batsman1 ? batsman1 : batsman2].Sixes = team1[batsmanstrick == batsman1 ? batsman1 : batsman2].Sixes + 1;
                       
                       
                    }
                    if (balls == 6)
                    {
                        balls = 0;
                        BowlersP = !BowlersP;
                        team1[batsmanstrick].NostrickOnOver = team1[batsman1].NostrickOnOver + 1;
                    }
                    //else
                    //{
                        if (runs > 0 && !(runs==4||runs==6) )
                            team1[batsmanstrick == batsman1 ? batsman1 : batsman2].NOstrickNotOnOver = team1[batsmanstrick == batsman1 ? batsman1 : batsman2].NOstrickNotOnOver + runs;
                    //}
                    
                    if (strickChange)
                    {
                        batsmanstrick = (batsmanstrick == batsman1) ? batsman2 : batsman1;
                    }

                    if (balls == 0 && batsmanNeedChange)
                    {
                        batsmanstrick = (batsmanstrick == batsman1) ? batsman2 : batsman1;
                    }
                    team2[BowlersP ? 0 : 1].Score = team2[BowlersP ? 0 : 1].Score + run;

                    totalScore = totalScore + (run);
                }

            }

        
            Console.WriteLine("Total Score :" + totalScore);

            foreach (var batsman in team1)
            {
                if (batsman.NOstrickNotOnOver > 0 || batsman.NostrickOnOver > 0)
                {

                 //   Console.WriteLine("No of times Batesman strick not on over in " + batsman.name + ":" + +batsman.NOstrickNotOnOver);

                    Console.WriteLine("Batsmen change the strike at the end of each over :  " + batsman.name + ":" + +batsman.NostrickOnOver);
                }
            }

            //  Console.WriteLine("Extra runs" + extraRuns);
            foreach (var batsman in team1)
            {
                if(batsman.Score>0)
                Console.WriteLine("Scores of batsmen " + batsman.name + ":" + +batsman.Score);

            }
            foreach (var batsman in team1)
            {
                if (batsman.NOstrickNotOnOver > 0 || batsman.NostrickOnOver > 0)
                {

                    Console.WriteLine("Batsmen change the strike at (not counting end of over) :  " + batsman.name + ":" + +batsman.NOstrickNotOnOver);

                }
            }
            Console.WriteLine("Bowler P runs" + team2[0].Score);

            Console.WriteLine("Bowler Q runs" + team2[1].Score);

            Console.WriteLine("Bowler P Extras" + team2[0].Extras);

            Console.WriteLine("Bowler Q Extras" + team2[1].Extras);

          
            Console.WriteLine("Total Wicket" + wickets);
            
            Console.ReadLine();
        }



        public class Team
        {
            public string name { get; set; }
            public int Score { get; set; }
            public bool Strick { get; set; }
            public int NostrickOnOver { get; set; }
            public int NOstrickNotOnOver { get; set; }
            public int Fours { get; set; }
            public int Sixes { get; set; }
            public int Extras { get; set; }
            public bool BatsmanOut { get; set; }


        }

        public class Rules
        {
            public string Rule { get; set; }
            public int ExtraRun { get; set; }
            public bool BatsmanRun { get; set; }
            public bool BallCount { get; set; }
            public bool BatsmanOut { get; set; }
            public bool NextBatsManFacingNewBall { get; set; }
        }
        public static class Teams
        {


            public static IEnumerable<Rules> GetRules()
            {

                List<Rules> rules = new List<Rules>();
                rules.Add(new Rules()
                {
                    Rule = "wd",
                    ExtraRun = 1,
                    BallCount = false,
                    BatsmanRun = false,
                    BatsmanOut = false,
                    NextBatsManFacingNewBall = false,

                });
                rules.Add(new Rules()
                {
                    Rule = "wn",
                    ExtraRun = 0,
                    BallCount = true,
                    BatsmanRun = false,
                    BatsmanOut = true,
                    NextBatsManFacingNewBall = true,
                });
                rules.Add(new Rules()
                {
                    Rule = "w",
                    ExtraRun = 0,
                    BallCount = true,
                    BatsmanRun = false,
                    BatsmanOut = true,
                    NextBatsManFacingNewBall = false,
                });
                rules.Add(new Rules()
                {
                    Rule = "nb",
                    ExtraRun = 1,
                    BallCount = false,
                    BatsmanRun = false,
                    BatsmanOut = false,
                    NextBatsManFacingNewBall = false,
                });
                rules.Add(new Rules()
                {
                    Rule = "b",
                    ExtraRun = 0,
                    BallCount = true,
                    BatsmanRun = false,
                    BatsmanOut = false,
                    NextBatsManFacingNewBall = false,

                });

                rules.Add(new Rules()
                {
                    Rule = "lb",
                    ExtraRun = 0,
                    BallCount = true,
                    BatsmanRun = false,
                    BatsmanOut = false,
                    NextBatsManFacingNewBall = false,

                });
                return rules;
            }
            public static List<Team> GetTeamIndia()
            {
                List<Team> team = new List<Team>();
                team.Add(new Team()
                {
                    name = "A"
                });
                team.Add(new Team()
                {
                    name = "B"
                });
                team.Add(new Team()
                {
                    name = "C"
                });
                team.Add(new Team()
                {
                    name = "D"

                });
                team.Add(new Team()
                {
                    name = "E"
                });
                team.Add(new Team()
                {
                    name = "F"
                });
                team.Add(new Team()
                {
                    name = "G"
                });
                team.Add(new Team()
                {
                    name = "H"
                });
                team.Add(new Team()
                {
                    name = "I"
                });
                team.Add(new Team()
                {
                    name = "J"
                });
                team.Add(new Team()
                {
                    name = "K"
                });
                return team;
            }

            public static List<Team> GetTeamAustralia()
            {
                List<Team> team = new List<Team>();
                team.Add(new Team()
                {
                    name = "P"
                });
                team.Add(new Team()
                {
                    name = "Q"
                });
                team.Add(new Team()
                {
                    name = "R"
                });
                team.Add(new Team()
                {
                    name = "S"

                });
                team.Add(new Team()
                {
                    name = "T"
                });
                team.Add(new Team()
                {
                    name = "U"
                });
                team.Add(new Team()
                {
                    name = "V"
                });
                team.Add(new Team()
                {
                    name = "W"
                });
                team.Add(new Team()
                {
                    name = "X"
                });
                team.Add(new Team()
                {
                    name = "Y"
                });
                team.Add(new Team()
                {
                    name = "Z"
                });

                return team;
            }


        }

    }
}