/* Date: 1.3.2014, Time: 14:08 */
using System;
using System.Drawing;
using NoxShared;

//using Microsoft.DirectX.DirectDraw;

namespace MapInterface
{
public struct IMAGE_SIZE
{
	public int X;
	public int Y;
	public int Width;
	public int Height;
}

public class SURFACE
{
  public int numChildren;
  public Bitmap surface;
  public int Image_ID;
  public int Width;
  public int Height;
  public int OffsetX;
  public int OffsetY;
  public VideoBag.Section.SectionEntry.Types ImgType;
  //NxImage img;
  
  //LIST<IMAGE_SIZE> Sizes;
  //int numSizes;

  public bool Remove()
  {
    if(numChildren>1)
	{numChildren--;return true;}
	else
	{
		//Sizes.Clear();
		/*if(name!=null)
		{
			name.ReleaseGraphics();
			name=null;
		}
		if(surface!=null)
		{
			surface.ReleaseGraphics();
			surface=null;
			return true;
		}*/
	}
  
  return false;
  }

  public SURFACE()
  {surface=null;numChildren=0;Image_ID=0;ImgType=0;}
  
}
}
