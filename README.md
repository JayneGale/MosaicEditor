# Mosaic Editor
A Windows Forms app to edit puzzles for the Velocity Studio "Mosaic" hex puzzle web app.

### Introduction
This application provides a simple graphical editor for hexagonal puzzle patterns.  It saves and
reads patterns in a file format understood by and shared by the Mosaic web app.

### User Interface
When started this application displays a window filled with blank triangles, grouped into hexagons.
The user can either load an existing pattern for editing (**File | Open**), or just start creating a
new pattern on the blank canvas.

The application also displays the available colour palette, as a set of coloured squares.  The palette includes
a "blank" colour (grey) and an eraser.

To create or edit a pattern, the user first clicks on any palette entry to choose a colour, then
clicks on any tile (triangle) of the pattern to change its colour.

The pattern can be saved at any time using the **File | Save** menu.

To exit the app select **File | Exit** or click on the standard Windows close button.

## Program Structure
The current pattern is stored in the data model, which comprises:

* A list of hexagons
* A **Hexagon** class, to represent each hexagon.  Each hexagon includes the 6 triangles within it.
* A **Triangle** class, to represent each triangle.

The file open and save operations read and write this model to disk, in the Mosaic puzzle format.

The puzzle is drawn on screen with a PictureBox component (pictureBox1).  When the application is running,
the pictureBox1_Paint event fires many times per second.  The application captures this event and
redraws the puzzle as defined by the current pattern data model (above).  In this way any change
in the data model is very quickly reflected on screen.

When the user clicks on any triangle to change its colour, the application captures the pictureBox1_Click()
event and works out which triangle was clicked upon.  It then updates that triangle's colour in the
data model.  The paint event handler (above) looks after the rest.

### Triangle Class
The triangle class includes the following properties:
* color
* orientation (right way up / upside down)
* bounds (a rectangle)
* parent (a hexagon)

And the following methods:
* draw() - draws this triangle
* contains() - returns true if p falls within this triangle

### Hexagon Class
The hexagon class includes the following properties:
* list of triangles (6)
* bounds (a rectangle)

And the following methods:
* draw() - draws this hexagon (including the triangles it contains)
* contains() - returns true if p falls within this hexagon

## Puzzle file format
(To be defined)




