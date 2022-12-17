/*
 * Brenden Blake
 * 10/20/2022
 * Tic-Tac-Toe
 * 
 * This is a program that dynamically creates an n x n tic tac toe board and allows the players to play by clicking on buttons
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        // the size of the tic-tac-toe board
        int size = 3;

        // the height and width of the board buttons
        const int BOARD_BUTTON_WIDTH = 75;
        const int BOARD_BUTTON_HEIGHT = 75;

        // the height of the play button
        // only the height is defined here, because the width is dependent on the size of the board
        const int PLAY_BUTTON_HEIGHT = 50;
        

        String currentPlayer = "X";
        Button playButton;
        Button[,] board;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Tic-Tac-Toe";
            // the form should increase proportionately to the size of the board
            this.Width = size * BOARD_BUTTON_WIDTH + 115;
            this.Height = size * BOARD_BUTTON_HEIGHT + 200;
            // initialize the board on form load
            initBoard();
        }

        // function to initialize the board
        private void initBoard()
        {
            // startX and startY are the starting positions for the first button
            int startX = 50;
            int startY = 50;
            board = new Button[size, size];
            // create board buttons and add them to the form
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    board[i,j] = createButton(new Point(startX, startY), "boardButton", BOARD_BUTTON_HEIGHT, BOARD_BUTTON_WIDTH);
                    this.Controls.Add(board[i, j]);
                    
                    // move to the left for the next button
                    startX += 75;
                }
                // reset the starting X position
                startX = 50;
                // move down to the next row
                startY += 75;
            }

            // create and add the play button to the form
            // the button's width is scaled to the size of the board
            playButton = createButton(new Point(startX, startY+25), "playButton", PLAY_BUTTON_HEIGHT, size * BOARD_BUTTON_WIDTH);
            this.Controls.Add(playButton);
        }

        // function to create a button
        private Button createButton(Point pos, String name, int height, int width)
        {
            Button button = new Button();
            button.Name = name;
            button.Location = pos;
            button.Height = height;
            button.Width = width;

            // if the button's name is playbutton, it isn't a board button
            // set the text and event handler accordingly
            if (name == "playButton")
            {
                button.Text = "Play Tic-Tac-Toe: " + currentPlayer + "'s turn";
                button.Click += new EventHandler(playButton_Click);
            }
            // if the button's name isn't playbutton, it is a normal board button
            // set the event handler
            else
            {
                button.Click += new EventHandler(boardButton_Click);
            }

            return button;
        }
        // function to handle the play button being clicked
        private void playButton_Click(object sender, EventArgs e)
        {
            // reset the board if the button is clicked
            resetBoard();
        }

        // function to handle the board buttons being clicked
        private void boardButton_Click(object sender, EventArgs e)
        {
            Button currentButton = ((Button)sender);
            // set the chosen button the symbol of the current player
            currentButton.Text = currentPlayer;
            // disable the chosen button
            currentButton.Enabled = false;

            // check if there is a win
            if(checkWin())
            {
                playButton.Text = currentPlayer + " wins! Click to Play Again!";
                // disable the buttons
                disableButtons();
            }
            // if there isn't a win
            // check if there is a draw
            else if(checkDraw())
            {
                playButton.Text = "Draw! Click to Play Again!";
                //disable the buttons
                disableButtons();
            }
            // if there isn't a win or draw, continue the game
            else
            {
                // if the player that just played is X, switch to O
                // otherwise switch to X
                if (currentPlayer == "X")
                {
                    currentPlayer = "O";
                }
                else
                {
                    currentPlayer = "X";
                }
                // Show the current player on the button below the board
                playButton.Text = currentPlayer + "'s turn";
            }
            
        }

        // function to reset the board after a win or draw
        private void resetBoard()
        {
            // set the current player to X
            currentPlayer = "X";
            // reset the playButton text to X's turn
            playButton.Text = "Play Tic -Tac -Toe: " + currentPlayer + "'s turn";
            // re-enable all of the buttons and reset their text to an empty string
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    board[i, j].Enabled = true;
                    board[i, j].Text = "";
                }
            }
        }
        // function to disable all of the board buttons after a win or draw
        private void disableButtons()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j].Enabled = false;
                }
            }
        }
        // function to check the rows for a win
        private bool checkRows()
        {
            bool winFound = false;
            int count = 0;
            // check rows
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    // if a square is found that is not equal to the current player
                    // this row is not a win, move on to the next row
                    if(board[i,j].Text != currentPlayer)
                    {
                        winFound = false;
                        count = 0;
                        break;
                    }
                    // if the square is equal to the current player
                    // continue checking the row
                    // add to the count, this is used to see if a full row was valid
                    count += 1;
                    winFound = true;
                }
                // if the count is equal to the size, a full row was equal to the current player
                // this means that there is a win
                if (count == size)
                {
                    break;
                }
            }
            count = 0;
            return winFound;
        }
        // function to check the columns for a win
        private bool checkCols()
        {
            bool winFound = false;
            int count = 0;
            // check cols
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    // if a square is found that is not equal to the current player
                    // this col is not a win, move on to the next col
                    if (board[j,i].Text != currentPlayer)
                    {
                        winFound = false;
                        count = 0;
                        break;
                    }
                    // if the square is equal to the current player
                    // continue checking the col
                    // add to the count, this is used to see if a full col was valid
                    count += 1;
                    winFound = true;
                }
                // if the count is equal to the size, a full col was equal to the current player
                // this means that there is a win
                if (count == size)
                {
                    break;
                }
            }

            count = 0;
            return winFound;
        }
        // function to check the diagonals for a win
        // if startRow, startCol is equal to 0,0, then it will check the diagonal from top left to bottom right
        // if startRow, startCol is equal to size-1, 0, then it will check the diagonal from bottom left to top right
        private bool checkDiagonals(int startRow, int startCol)
        {
            int row = startRow;
            int col = startCol;
            bool winFound = false;
            int count = 0;

            // check the diagonal from (0,0) to (size-1, size-1)
            if(startRow == 0 && startCol == 0)
            {
                // while the row is less than the size, we can keep checking the diagonal
                // we only need to check row, because row and col will always be the same
                while(row < size)
                {
                    if(board[row,col].Text != currentPlayer)
                    {
                        winFound = false;
                        count = 0;
                        break;
                    }

                    count += 1;
                    row += 1;
                    col += 1;
                    winFound = true;

                    if (count == size)
                    {
                        break;
                    }
                }
            }
            // check the diagonal from (size-1, 0) to (0, size-1)
            else
            {
                // while the row is greater than 0, we can keep checking the diagonal
                while (row >= 0)
                {
                    if(board[row, col].Text != currentPlayer)
                    {
                        winFound = false;
                        count = 0;
                        break;
                    }

                    count += 1;
                    row -= 1;
                    col += 1;
                    winFound = true;

                    if(count == size)
                    {
                        break;
                    }
                }
            }

            count = 0;
            return winFound;
        }

        // function that uses all the checks above to decide if there is a win or not
        private bool checkWin()
        {
            return checkRows() || checkCols() || checkDiagonals(0,0) || checkDiagonals(size-1, 0);
        }

        // function to check if there is a draw
        // this will only occur if there isn't a win
        private bool checkDraw()
        {
            int count = 0;
            // iterate through the board and check if all the buttons have been chosen
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    if(board[i,j].Text == "X" || board[i,j].Text == "O")
                    {
                        count += 1;
                    }
                }
            }
            // if the count is equal to the dimensions of the array, then all the buttons have been clicked
            // this means there is a draw
            return count == size*size;
        }
    }
}
