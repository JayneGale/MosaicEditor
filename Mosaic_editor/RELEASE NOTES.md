## Mosaic Editor Release Notes

Changes to the Mosaic Editor, most recent first:

### 2021-06-09
- TODO: Finish the "Centre on this tile" menu function. DONE.
- TODO: Add drag/drop to the file tree. DONE.

### 2021-05-12
- BUG: When first run, this version crashes because the PuzzleFolders setting is null.  Fixed.
- BUG: After a saved puzzle is reloaded, Any grey tiles disappear the first time you click on one.  Fixed.

### 2021-05-05
- FEATURE: added option to display tile coordinates in File | Preferences.

### 2020-04-28
- FEATURE: Added a file browser pane, so you can easily see all the current puzzle files.
  - click on any puzzle file to load it.
  - the new File | Preferences screen lets you change the root folder.
  - only one level of subfolders is displayed, as this is all the editor will process.
- added "isDirty" property to Puzzle, to be used to prompt user before closing, etc.

### 2021-04-21
- CHANGE: removed puzzle "difficulty" attribute, added "tier" to replace it.
- FEATURE: a new File | Preferences dialog lets you set the working puzzle folder, and the Mosaic Engine folder.
  - The working puzzle folder is the one where individual puzzles are saved, in subfolders names "tier 1", "tier 2", etc.
  This is also the folder into which the full set are a saved, as "puzzleSet1.js".  You can maintain more than one set of
puzzles by using more than one working folder, each with its own tier subfolders.
  - The Mosaic Engine folder is the folder where that puzzle set file is copied, to bring it into action.
- CHANGE: The File | Save dialog now lets you specify "tier".
- CHANGE: The File | Save process now scans the "tier N" subfolders for puzzle files when building the full puzzle set.  All puzzles must be in one of those folders.
- Puzzle files in each tier folder are read in alphabetic order, and this order determines the level number of each.
- Tier folders are added to the puzzle set in  alphabetic order, and this determines their tier number in the engine.

### 2021-04-15
- CHANGE: the editor now centres its display on tile [0,0], and no longer tries to "re-center" or anything like that.  Changed the code which calculates tile positions accordingly.
- CHANGE: added the "Save" dialog box allowing the setting of the properties of the puzzle before saving.
- CHANGE: added the "Copy to Mosaic Engine" file menu option to simplify deployment (but only on my setup).
- CHANGE: added PuzzleDifficulty and PuzzleType enumerations (in Constants.cs)\
- CHANGE: changed the hatching style used on fixed tiles to improve readability.
- CHANGE: added a 'centre' function to allow you to specify the centre tile of the puzzle, i.e. the tile around which this puzzle should spin.

### 2021-03-31
- BUG: after loading a puzzle, changing any colour in the colour picker does not immediately refresh the colours on the puzzle.  Fixed.
- TODO: control-click to select the colour from any triangle.  Done.
- TODO: get the javascript client ("Mosaic Puzzle Engine" sorry!) going again with this new file format.
- BUG: clearing the puzzle does not clear the "fixed" hexangles.
- FEATURE: made the display of "fixed" hexangles clearer.  Gold outline, and a lighter hatch pattern.  Done.

### 2021-03-24
- GRATUITOUS CHANGE: redesigned the coordinate system to the new 'no holes' version.  Fixed all the resulting crashes.
- BUG: reloading the colour picker triggers a picturebox resize event! (because it clears the toolbar briefly).  Fixed.
- BUG: puzzles display correctly when reloaded, UNTIL you click anywhere, when all other hexes disappear.  Fixed.  Caused
by not setting coloured triangles "active" when reloading from files.
- BUG: after loading a puzzle, changing any colour in the colour picker does not immediately refresh the colours on the puzzle.

### 2021-03-17
- FEATURE: made the menu shortcut keys work: Ctrl-O for "Open", etc.
- when a window is resized the current puzzle disappears.  This happens when the "Puzzle.recentre()" method runs.
It is wrongly moving the active tiles completely off screen, instead of correctly centering them.  Fixed.
- BUG: the editor draws slightly more hexagons than actually fit in the window, overflowing the right
and bottom sides.  Fixed.
- BUG: when a puzzle is saved and reloaded the colours are lost.  Fixed.
- BUG: the save function should only save "active" hexagons; not all hexagons within the active bounds.
- BUG: when a puzzle is loaded it displays, but disappears on the first mouse click!
- BUG: the saved puzzle (.json) file filename is not set.  Fixed.
- BUG: when a palette colour is changed the puzzle display does not immediately update.  Fixed.  The colour picker now
raises a custom "onPaletteChanged" event which I use to call PictureBox1.Invalidate(), triggering a display refresh.
- FEATURE: mouse wheel now zooms in and out, changing the grid size.

### 2021-03-10
- I split ColourPalette class into ColourPalette and ColourPicker classes.  ColourPicker implements
the user interface (toolbar colour selector).  ColourPalette is just a set of colours.
- in ColourPalette changes the set of colors from an Array to a List.
- simplified the exported json puzzle file by making more of the properties of Puzzle,
Hexagon and Triangle "internal" rather than "public".  This is to make it more digestable to
the puzzle engine.  To counter this a bit more work is done when the puzzle is loaded to fill
in any missing properties.
- instead of each Triangle having a "color" property it now has a "colorNo" property, which is
an index into the current palette.  The palette is included in the output .json file as a property
of the puzzle.
- changed the default save folder from \MyDocuments to \MyDocuments\mosaic.
- added a few more properties to Puzzle: repeats; dontMangle; fixedColours.
- added a PuzzleMerger class which merges all .json puzzle files in the output folder into a
single puzzleSet.js javascript file, ready for use in the Mosaic engine.  This runs every time a
puzzle is saved, so puzzleSet.js is always up to date.
- I currently have to manually copy puzzleSet.js to the Mosaic puzzle engine project when I want
to update it.
- removed the unnecessary "Save As" menu option.

### 2021-03-02
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


