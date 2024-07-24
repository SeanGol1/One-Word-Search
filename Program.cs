
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using Wordsearch;

class Wordsearch_Create
{
    private static char[] letters = ['F', 'O', 'X'];
    public static string word = "FOX";
    public const int gheight = 25;
    public const int gwidth = 25;
    public static List<GridLetter> GridLetters = new List<GridLetter>();
    public static List<char> failedChars = new List<char>();
    public static int failureCounter = 0;
    public static int resetCounter = 0;
    public const int maxResets = 100;
    public static int answerInt = 0;
    public static bool answerTrue = false;

    // Main Method
    public static void Main(String[] args)
    {
        Console.WriteLine("START - Enter 3 letter word:");
        word = Console.ReadLine().ToUpper();
        letters = word.ToCharArray();
        
        Random rnd = new Random();
        double min = ((gheight * gwidth) / 100);
        double max = ((gheight * gwidth) / 100) * 10;
        int rand = rnd.Next((int)min, (int)max);
        answerInt = rand == 0 ? 1 : rand;
        StartLoop();        
        PrintGrid();
        Console.WriteLine(answerTrue);
        Console.WriteLine("END");
    }

    public static void StartLoop()
    {
        GridLetter lastitem = GridLetters.Count() != 0 ? GridLetters.Last() : AddLetter();
        while ((lastitem.X != (gheight - 1)) || (lastitem.Y != (gwidth - 1)))
        {
            lastitem = AddLetter();
            failedChars.Clear();
        }

        if (resetCounter == maxResets)
        {
            Console.WriteLine("Too many reset.");
            return;
        }

        if (!answerTrue && resetCounter < maxResets)
        {
            resetCounter++;
            GridLetters.Clear();
            StartLoop();
        }

        
        
    }

    private static GridLetter AddLetter()
    {
        //get random letter to add.
        var untriedChars = letters.Except(failedChars).ToList();        
        Random rnd = new Random();
        char letter = untriedChars[rnd.Next(untriedChars.Count)];
        

        (int,int) coord;
        
        
        //new grid
        if (GridLetters.Count() < 1)
        {
            coord = (0, 0);
            GridLetter gl = new GridLetter(coord.Item1, coord.Item2, letter);
            GridLetters.Add(gl);
            return gl;
        }

        GridLetter lastitem = GridLetters.Last();

        // new line
        if (lastitem.Y >= (gwidth-1))
        {
            coord = (lastitem.X + 1, 0);
        }
        else { 
            coord= (lastitem.X, lastitem.Y + 1);
        }

        //check if it fits
        GridLetter gle = new GridLetter(coord.Item1, coord.Item2, letter);
        if (VerificationCheck(gle))
        {
            GridLetters.Add(gle);
        }
        else
        {
            failureCounter++;
            if (answerInt != failureCounter)
            {
                //save current letter and change
                if (failedChars.Count < word.Length - 1)
                {
                    failedChars.Add(letter);
                    AddLetter();
                }
                else
                {
                    GridLetters.Remove(lastitem);
                    lastitem = GridLetters.Last();
                    AddLetter();
                }
            }
            else
            {
                GridLetters.Add(gle);
                answerTrue = true;
            }
        }

        return gle;
    }

    public static void PrintGrid()
    {
        StringBuilder grid = new StringBuilder();
        StringBuilder line = new StringBuilder();

        //each line.
        for (int i = 0; i < gheight; i++) {
            foreach (GridLetter letter in GridLetters.Where(l => l.X == i))
            {
                line.Append(letter.Letter.ToString());
                line.Append("  ");
            }
            grid.AppendLine(line.ToString());
            line = new StringBuilder();
        }

        Console.WriteLine(grid.ToString());
    }

    public static bool VerificationCheck(GridLetter gl)
    {
        // 0   1   2   3   4 
        // 5   6   7   8   9
        // 10  11 [12] 


        // 0  X  1  X  2
        // X  3  4  5  X
        // 6  7  [8]

        bool isValid = false;
        Point[] grid = new Point[9];
        grid[8] = new Point(gl.X, gl.Y);

        grid[0] = new Point((gl.X - 2) < 0 ? -1 : (gl.X - 2)                
            , (gl.Y - 2) < 0 ? - 1 : (gl.Y - 2));
        grid[1] = new Point(gl.X                
            , (gl.Y - 2) < 0 ? -1 : (gl.Y - 2));
        grid[2] = new Point((gl.X - 2) > (gwidth - 1) ? -1 : (gl.X - 2)
            , (gl.Y + 2) > (gheight - 1) ? -1 : (gl.Y + 2));
        grid[3] = new Point((gl.X - 1) < 0 ? -1 : (gl.X - 1)
            , (gl.Y - 1) < 0 ? -1 : (gl.Y - 1));
        grid[4] = new Point(gl.X
            , (gl.Y - 1) < 0 ? -1 : (gl.Y - 1));
        grid[5] = new Point((gl.X + 1) > (gwidth - 1) ? -1 : (gl.X + 1)
            , (gl.Y + 1) > (gheight - 1) ? -1 : (gl.Y + 1));
        grid[6] = new Point((gl.X - 2) < 0 ? -1 : (gl.X - 2)
            , gl.Y);
        grid[7] = new Point((gl.X - 1) < 0 ? -1 : (gl.X - 1)
            , gl.Y);


        //   0
        //       3
        //           8

        if(isAnswerWord(grid[0], grid[3], gl.Letter)) return false;
        //if(isAnswerWord(grid[8], grid[3], grid[0])) return false;


        //          2
        //     5
        //  8

        if (isAnswerWord(grid[2], grid[5], gl.Letter)) return false;
        //if (isAnswerWord(grid[8], grid[5], grid[2])) return false;


        //      1
        //      4
        //      8

        if (isAnswerWord(grid[1], grid[4], gl.Letter)) return false;
        //if (isAnswerWord(grid[8], grid[4], grid[1])) return false;

        //
        //  
        //  6   7  8

        if (isAnswerWord(grid[6], grid[7], gl.Letter)) return false;
        //if (isAnswerWord(grid[8], grid[7], grid[6])) return false;

        //if you made it here you must have passed so return true. 

        return true;
    }

    private static char GetLetter(Point p)
    {
        try
        {
            GridLetter letter = GridLetters.Where(gl => gl.X == p.X && gl.Y == p.Y).FirstOrDefault();
            if (letter != null) return letter.Letter;   

            return char.MinValue;
            //string x = GridLetters.Where(gl => gl.X == p.X && gl.Y == p.Y).Select(gl => gl.Letter).FirstOrDefault().ToString();
            //char y = char.Parse(x);
            //return Convert.ToChar(x);
        }
        catch
        {
            return char.MinValue;
        }
    }

    private static bool ValidPoint(Point p) {
        if(p.X == -1 || p.Y == -1)
            return false;
        else
            return true;

        
    }

    private static bool isAnswerWord(Point p1, Point p2, char letter)
    {
        //bool isValid = false;
        if (ValidPoint(p1) && ValidPoint(p2))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetLetter(p1).ToString());
            sb.Append(GetLetter(p2).ToString());
            sb.Append(letter);

            string x = sb.ToString();
            if (sb.ToString() == word) return true;
            
            //check Reverse
            sb = new StringBuilder();
            sb.Append(letter);
            sb.Append(GetLetter(p2).ToString());
            sb.Append(GetLetter(p1).ToString());

            string y = sb.ToString();
            if (sb.ToString() == word) return true;
        }

        return false;

    }




}