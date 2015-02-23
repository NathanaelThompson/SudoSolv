//Nathanael Thompson
//Artificial Intelligence, Prof. Franklin
//SPSU Spring 2015
//Assignment 0 - Sudoku Solver

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

//To continue this project, add conjecture logic to the tail end of the SolverEngine algorithm.
namespace SudokuSolver_NThompson
{
    class SudoSolv
    {
        public static SudokuCell[,] sudokuGrid = new SudokuCell[9, 9];
        public static SudokuCell[,] solutionGrid = new SudokuCell[9, 9];
        
        ////////////
        ////Main////
        ////////////
        
        static void Main(string[] args)
        {
            Console.WriteLine("SudoSolv");
            Console.WriteLine("---------");
            string puzzleNum;
            
        Restart:
            ;
            
            //The next 20 or so lines deals with the user's input of puzzle choice
            Console.WriteLine("Enter a number between 1 and 5, or leave the space blank for a random puzzle.");
            puzzleNum = Console.ReadLine();
            puzzleNum = puzzleNum.Trim();
            try
            {
                Random rand = new Random();
                int puzNumber = rand.Next(1, 6);

                //if the user enter iffy input, fix it here
                if (puzzleNum == "" || puzzleNum == " "||int.Parse(puzzleNum) < 1 || int.Parse(puzzleNum) > 5)
                {
                    if (puzzleNum == "" || puzzleNum == " ")
                    {
                        Console.WriteLine("You entered nothing, so puzzle {0} was selected.", puzNumber.ToString());
                    }
                    else
                    {
                        Console.WriteLine("You entered {0}, but puzzle {1} was selected.", puzzleNum, puzNumber.ToString());
                    } 
                    puzzleNum = puzNumber.ToString();
                }
            }
            catch (Exception ex)
            {
                //if the user enters invalid input, tell the user and restart
                Console.WriteLine(ex.Message);
                Console.WriteLine("Restarting...");
                goto Restart;
            }

            //Once a puzzle is selected, ask the user to begin the solver
            SudokuInitializer(int.Parse(puzzleNum));
            Console.WriteLine("Press any key to begin solving.");
            Console.ReadLine();

            //The main workhorse of this program
            SolverEngine();
            
            //After SolverEngine has completed,
            Console.WriteLine("Press any key to restart, or type 'exit' to quit.");
            string exitInput = Console.ReadLine();
            if (exitInput != "exit")
            {
                
                goto Restart;
            }
           
        }
        public static int GatherInformation(SudokuCell currentCell)
        {
            int xPos = currentCell.xPos;
            int yPos = currentCell.yPos;
            int removeCount = 0;
            if (currentCell.cellValue == "x")
            {
                for (int i = 0; i < 9; i++)
                {
                    //look at the current cell's row
                    if (sudokuGrid[i, currentCell.yPos].cellValue != "x")
                    {
                        currentCell.potentialValues.Remove(sudokuGrid[i, currentCell.yPos].cellValue);
                        removeCount++;
                    }

                    //look at the current cell's column
                    if (sudokuGrid[currentCell.xPos, i].cellValue != "x")
                    {
                        currentCell.potentialValues.Remove(sudokuGrid[currentCell.xPos, i].cellValue);
                        removeCount++;
                    }
                }

                //finding the local grid based of coordinate ranges (x, y)
                //grid 1: (0, 0) U (2, 2) grid 2: (0, 3) U (2, 5) grid 3: (0, 6) U (2, 8)
                //etc...

                int localGrid = 0;
                if (xPos >= 0 && xPos <= 2)
                {
                    if (yPos >= 0 && yPos <= 2)
                        localGrid = 1;
                    else if (yPos >= 3 && yPos <= 5)
                        localGrid = 2;
                    else
                        localGrid = 3;
                }
                else if (xPos >= 3 && xPos <= 5)
                {
                    if (yPos >= 0 && yPos <= 2)
                        localGrid = 4;
                    else if (yPos >= 3 && yPos <= 5)
                        localGrid = 5;
                    else
                        localGrid = 6;
                }
                else
                {
                    if (yPos >= 0 && yPos <= 2)
                        localGrid = 7;
                    else if (yPos >= 3 && yPos <= 5)
                        localGrid = 8;
                    else
                        localGrid = 9;
                }

                //Remove potential values found in the local grid
                //How cool does this switch look?
                //That's like textbook-quality code

                switch (localGrid)
                {
                    case 1:
                        for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 2:
                        for (int i = 0; i < 3; i++)
                            for (int j = 3; j < 6; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 3:
                        for (int i = 0; i < 3; i++)
                            for (int j = 6; j < 9; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 4:
                        for (int i = 3; i < 6; i++)
                            for (int j = 0; j < 3; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 5:
                        for (int i = 3; i < 6; i++)
                            for (int j = 3; j < 6; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 6:
                        for (int i = 3; i < 6; i++)
                            for (int j = 6; j < 9; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 7:
                        for (int i = 6; i < 9; i++)
                            for (int j = 0; j < 3; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 8:
                        for (int i = 6; i < 9; i++)
                            for (int j = 3; j < 6; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    case 9:
                        for (int i = 6; i < 9; i++)
                            for (int j = 6; j < 9; j++)
                                if (sudokuGrid[i, j].cellValue != "x")
                                {
                                    currentCell.potentialValues.Remove(sudokuGrid[i, j].cellValue);
                                    removeCount++;
                                }
                        break;
                    default:
                        break;
                }
                return removeCount;
                
            }
            else
            {
                return 0;
            }
        }
        public static void SolverEngine()
        {
            bool finished = false;
            bool failed = false;

            //the 80/20 loop
            while (!finished && !failed)
            {
                int removedCount = 0;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        //If a cell's value is unknown...
                        if (sudokuGrid[i, j].cellValue == "x")
                        {
                            //...but it only has one potential value...
                            if (sudokuGrid[i, j].potentialValues.Count == 1)
                            {
                                //...by process of elimination, the last potential value must be the actual value
                                sudokuGrid[i, j].cellValue = sudokuGrid[i, j].potentialValues[0];
                            }
                            else if (sudokuGrid[i, j].potentialValues.Count > 1)//...and it has multiple potential values...
                            {
                                //...try to gather more information.
                                removedCount += GatherInformation(sudokuGrid[i, j]);
                            }
                            else //...but has no potential values...
                            {
                                //...then the solver has failed.
                                failed = true;
                                break;
                            }
                        }
                    }
                }
                //To continue this project, insert conjecture logic here
                if (CheckForWin())
                {
                    Console.WriteLine("Puzzle solved");
                    Console.WriteLine("--------------");
                    PrintWin();
                    finished = true;
                }
                //else if (removedCount == 0 || failed)//This logic is currently incorrect, fail states CANNOT be detected
                //{
                //    Console.WriteLine("Solver failed");
                //    PrintWin();
                //    Console.ReadLine();
                //    break;
                //}
                removedCount = 0;
            }
        }
        public static void PrintWin()
        {
            //If a win state is detected, 
            //print the solved puzzle
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(sudokuGrid[i, j].cellValue);
                }
                Console.Write("\n");
            }
        }
        public static bool CheckForWin()
        {
            //If any cell's value is equal to the literal "x",
            //or if the solution grid doesn't match up,
            //return false, else, return true
            bool win = true;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (sudokuGrid[i, j].cellValue == "x")
                    {
                        win = false;
                    }

            if (win)
            {
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        if (sudokuGrid[i, j].cellValue != solutionGrid[i, j].cellValue)
                            win = false;

                if (!win)
                    Console.WriteLine("Solution doesn't match conjecture.");
            } 
            return win;
        }
            
        //Parses file input and creates the sudoku puzzle and solution grid
        public static void SudokuInitializer(int puzzleNumber)
        {
            string rawDataPuz, rawDataSol;
            string filePath = "";
            string solPath = "";
            string[] sanitizedInputPuzzle;
            string[] sanitizedInputSol;
            char[] delimiters = {',', '\n', '\r', '\t'};
            
            switch (puzzleNumber)
            {
                case 1:
                    filePath = "puzzles/puzzle183easy.txt";
                    solPath = "solutions/solution183easy.txt";
                    break;
                case 2:
                    filePath = "puzzles/puzzle185easy.txt";
                    solPath = "solutions/solution185easy.txt";
                    break;
                case 3:
                    filePath = "puzzles/puzzle187easy.txt";
                    solPath = "solutions/solution187easy.txt";
                    break;
                case 4:
                    filePath = "puzzles/puzzle101easy.txt";
                    solPath = "solutions/solution101easy.txt";
                    break;
                case 5:
                    filePath = "puzzles/puzzle102easy.txt";
                    solPath = "solutions/solution102easy.txt";
                    break;
                //case 6:
                //    filePath = "puzzle147med.txt";
                //    break;
                //case 7:
                //    filePath = "puzzle159med.txt";
                //    break;
                //case 8:
                //    filePath = "puzzle160med.txt";
                //    break;
                //case 9:
                //    filePath = "puzzle177med.txt";
                //    break;
                //case 10:
                //    filePath = "puzzle178med.txt";
                //    break;
                default:
                    break;
            }

            //reading and splitting the puzzle file
            rawDataPuz = File.ReadAllText(filePath);
            sanitizedInputPuzzle = rawDataPuz.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            //reading and splitting the solution file
            rawDataSol = File.ReadAllText(solPath);
            sanitizedInputSol = rawDataSol.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine("Puzzle before solution:");
            Console.WriteLine("-----------------------");

            int inputCounter = 0;

            //sets the relevant values for each cell in the sudoku grid
            for (int i = 0; i < 9; i++)
            {
                
                for (int j = 0; j < 9; j++)
                {
                    //unsolved grid
                    sudokuGrid[i, j] = new SudokuCell();
                    sudokuGrid[i, j].cellValue = sanitizedInputPuzzle[inputCounter];
                    sudokuGrid[i, j].xPos = i;
                    sudokuGrid[i, j].yPos = j;
                    
                    //solved grid
                    solutionGrid[i, j] = new SudokuCell();
                    solutionGrid[i, j].cellValue = sanitizedInputSol[inputCounter];
                    sudokuGrid[i, j].xPos = i;
                    sudokuGrid[i, j].yPos = j;

                    //assigns potential values (1-9) to cells with unknown values
                    if (sudokuGrid[i, j].cellValue == "x")
                    {
                        for (int k = 1; k < 10; k++)
                        {
                            sudokuGrid[i, j].potentialValues.Add(k.ToString());
                        }
                    }
                    inputCounter++;
                    Console.Write(sudokuGrid[i, j].cellValue);
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }
        public class SudokuCell
        {
            //backing fields
            private string cellVal;
            private List<string> potenVals;
            private int xPoint, yPoint;
            
            //SudokuCell Properties
            public List<string> potentialValues
            {
                get
                {
                    return potenVals;
                }
                set
                {
                    potenVals = value;
                }
            }
            public string cellValue
            {
                get
                {
                    return cellVal;
                }
                set
                {
                    cellVal = value;
                }
            }
            public int xPos
            {
                get
                {
                    return xPoint;
                }
                set
                {
                    xPoint = value;
                }
            }
            public int yPos
            {
                get
                {
                    return yPoint;
                }
                set
                {
                    yPoint = value;
                }
            }

            //default constructor
            public SudokuCell()
            {
                cellVal = "";
                potenVals = new List<string>();
                xPoint = -1;
                yPoint = -1;
            }
        }
    }
}
