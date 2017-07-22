# Sudokungfu
Solves Sudoku puzzles. Shows details of how each value was found.

### Background
Sudoku is addicting logic puzzle that requires you to fill out a 9 x 9 *cell* grid with numbers. The grid is broken down into three types of *sets*: nine rows, nine columns, and nine 3 x 3 boxes. Each set must contain the values 1 - 9 exactly once. The official rules and some variants can be found here: https://en.wikipedia.org/wiki/Sudoku. The purpose of Sudokungfu is to solve Sudokus and expose the logic used to solve them in a human readable way.

### How does it work?
Enter your unsolved Sudoku into Sudokungfu and hit *Enter*. 

Once the Sudoku is solved, you can double click on any cell to see the details of how the value in that cell was found.

### Advanced Techniques
The logic that comes into play when solving a difficult Suokdu can get quite complicated. The following advanced logic techniques are used by Sudokungfu to help solve difficult Sudokus.

#### Two/Three Spot Overlap Technique
If there is only two/three possible places for a value in set A, and those places overlap set B, then the value cannot go anywhere else in set B. 

#### Two/Three Spot Closure Technqiue
Within a set, if n values can only go in the same n cells, then nothing else can go in the n cells except one of the n values.

#### Possible Spot Shadow Technique
If there is only two possible places for a value in set A and in set B AND the two possible places are in the same two rows or the same two columns, then the value can go nowhere else in the two rows or the two columns.


Created by Jeff ten Have 2017.
