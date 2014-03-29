/* Date: 1.3.2014, Time: 13:55 */
using System;
using System.Drawing;
using Font = System.Drawing.Font;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MapInterface
{

public class GUI_DISPLAY
{

    public Font GuiFont; // The standard font for the game

	public bool Reload_Images;
    public bool Is_Splash;
	public bool Is_Loading;
	public bool Is_Minimized;

	public bool Can_Draw_GDI;

	public int WINDOW_WIDTH;
	public int WINDOW_HEIGHT;
	public int WINDOW_X;
	public int WINDOW_Y;

	public int MOUSE_X;
	public int MOUSE_Y;

    public bool ShowCur;

	public Control win;
	
	private Font defaultFont;
    
	/*public uint Get_Display_Status()
	{
		return( Display.STATUS_FLAGS );
	}
	public void Set_Display_Status(uint newVal)
	{
        Display.STATUS_FLAGS = newVal;
	}*/
	
	private double Detor(double angle)
	{
		return ((angle)*(3.14159))/180;
	}
	
	public bool RotatePoint(ref int X, ref int Y,double Degree)
	{
	   double X2=X;
	   Degree = Detor(Degree);
	
	   X = (int)(X*Math.Cos(Degree) - Y*Math.Sin(Degree));
	   Y = (int)(X2*Math.Sin(Degree) + Y*Math.Cos(Degree));
	
	return true;
	}

public bool Create(int winX,int winY,Control win2)
{

	if(win2 != null)
	{
	  win=win2;
	}

  Rectangle rec = win.ClientRectangle;;

  WINDOW_WIDTH=rec.Width;
  WINDOW_HEIGHT=rec.Height;
  WINDOW_X=winX;
  WINDOW_Y=winY;
  

return(true);
}

/**********************************************************************

Function: Render View

**********************************************************************/
public void Render(Graphics gr)
{
	
}

public GUI_DISPLAY(int WIN_WIDTH,int WIN_HEIGHT)
{
  WINDOW_WIDTH = WIN_WIDTH;
  WINDOW_HEIGHT = WIN_HEIGHT;
}

public GUI_DISPLAY()
{
  WINDOW_WIDTH = 0;
  WINDOW_HEIGHT = 0;
  ShowCur = false;
  Can_Draw_GDI = false;
  Is_Minimized = false;
  Reload_Images = false;
}

}
}
