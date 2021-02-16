using Mosaic_editor.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic_editor
{
    class PhotoInput
    {
        internal static Puzzle LoadImage(Bitmap image, int puzzleWidth = 10)
        {
            // TODO!
            // width and height of each triangle
            Rectangle imageRect = new Rectangle(0, 0, image.Width, image.Height);
            double triWidth = (image.Width / puzzleWidth);
            double triHeight = triWidth * Constants.COS30;
            bool upsidedown = true; // start with the first top left upside down triangle is (00)
            // the first rightside up triangle is (0,1)and so on

            // puzzleHeight is the number of rows of triangles
            // this is determined by the image ratio
            double imageRatio = image.Height / image.Width;
            double puzzleHeight = imageRatio * triWidth;
            var puzzleRows = Math.Floor(puzzleHeight);

            // now iterate through all the pixels in the image 
            // identify which triangle the pixel is in and add it to that trangle's array
            for (int col = 0; col < image.Height; col++)                
            {

                for (int row = 0; row < image.Width; row++)
                {
                    // image.GetPixel(row, col);

                    // make some new contains function that allocates find which triangle this pixel is in (triangle i, j)
                    // (triPixels[i,j] addPixel (row, column) 

                    //                    add its RGB properties to that triangles array of pixels 

                    // the triangles run from 0 to puzzleWidth - 1 across
                    //after reaching puzzleWidth, upsidedown = ! upsidedown
                    //and 0 to puzzleRows-1 for all rightside up triangles
                    //and 0 to puzzleRows-1 for all upside down triangles
                    // current definition - number triangle rows 
                }
                upsidedown = !upsidedown;
                puzzleRows += 1;
            }
            // foreach triangle in Triangles
            // foreach pixel in TriPixels[triangle]

            // { average or do some other thing to the RGB values



            ;
            throw new NotImplementedException();
        }
    }
}
