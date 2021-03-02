## Mosaic Editor Release Notes

Changes to the Mosaic Editor, most recent first:

2021-03-02
- added this Release Notes file.
- reduced default grid size from 100 to 70 pixels
- refactored the Hexagon constructor so that the Heagon list is populated first,
then the position of each hexagon (and its triangles) calculated.  This allowed the
grid to be resized without losing the current puzzle contents.  Related changes included:
    - changed the constructor signature so it just needs row and column
    - separated the calculation of bounding box etc. out in the refreshPosition() method
    - added an optional "force" parameter to method calculateOutline()
- lightened the outline colour of triangles, and darkened that of hexagons, to make the
outline of every hexagon more obvious.
- changed the grid so that the first row of hexagons is indented.
- added an index property to class Triangle, which is set to 0..5 to number each triangle.
- added code to draw the index number of each triangle on screen to help with debugging.  This
is turned off and on by a new "verbose" property.
- added code to draw the row and column number of each hexagon on screen to help with debugging.  This
is turned off and on by a new "verbose" property.
- in Form1.cs added code to automatically redraw the grid every time the window is resized,
without losing the current puzzle design.  This is handled by a new "recentre()" method
because it is also intended to recentre the puzzle on the grid, although it does not do this
yet.

The code has large blocks which were commented out during the above changes, to be tidied up
in future.


