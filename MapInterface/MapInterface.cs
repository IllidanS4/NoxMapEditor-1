/* Date: 1.3.2014, Time: 13:35 */
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NoxShared;

namespace MapInterface
{
	public class MAP_GUI
	{
		private NOX_MAP Map;
		private Control win;
		
		public MAP_GUI(string noxdir)
		{
			Map = new NOX_MAP(noxdir);
		}

		public int Scrollx;
		public int Scrolly;
		public int MOUSEX;
		public int MOUSEY;
		public int MAG_MAX;
		public int MAG_SIZE;

		public bool Scale(bool on)
		{
			return false;
           //return(Map.Scale(on));
		}
		public int GetCenterX()
		{
			return(Map.X + Map.Display.WINDOW_WIDTH/2);
		}
		public int GetCenterY()
		{
			return(Map.Y + Map.Display.WINDOW_HEIGHT/2);
		}

		public void Magnify(int MAX,int SIZE) // This needs a total re-write	
		{
           Map.MAG_MAX = 550-MAX; // Where does the 550 come from?
		   Map.MAG_SIZE = SIZE;
		   MAG_MAX = MAX;
		   MAG_SIZE = SIZE;
		}
		
		public void SetMouse(int X, int Y)
		{
			Map.SetMouse((int)X,(int)Y);	
		}
		
	public void RemoveTile( int X, int Y) // Remove tile
	{
		Map.Floor.tileMap[Y,X] = default(TILE_INFO);
		/*Map.Floor.tileMap[Y,X].TileImg = null;
        Map.Floor.tileMap[Y,X].pTile = null;*/
		//memset(&Map.Floor.tileMap[Y,X],0x00,sizeof(TILE_INFO));
		Map.Floor.tileMap[Y,X].Tile = (-1);
	}
	public void SetBG(Color color)
	{
		Map.BGColor = color;
	}
	public void ClearMap()
	{
		Map.Floor.Clear();
		Map.Objects.Clear();
		Map.X = 0;
		Map.Y = 0;
	}
	public void ClearTiles()
	{
		Map.Floor.Clear();
	}
	public void ClearObjects()
	{
		Map.Objects.Clear();
	}

    public void SetLoc(int X, int Y)//, int &lastX, int &lastY)
	{		
		Map.X = X - Map.Display.WINDOW_WIDTH/2;
	    Map.Y = Y - Map.Display.WINDOW_HEIGHT/2;

	   if(((Map.X+1)/23)>253-(Map.Display.WINDOW_WIDTH/23))
	   {Map.X=23*(253-(Map.Display.WINDOW_WIDTH/23));}
  	   else if(((Map.X+1)/23)<2)
	   {Map.X=46;}  
   
	   if(((Map.Y+1)/23)>(253)-(Map.Display.WINDOW_HEIGHT/23))
	   {Map.Y=23*((253)-(Map.Display.WINDOW_HEIGHT/23));}
  	   else if(((Map.Y+1)/23)<1)
	   {Map.Y=46;}
	}
	public bool SetXY(int X, int Y)//, int &lastX, int &lastY)
	{	
       Map.X += X;
	   Map.Y += Y;
       
	   if(((Map.X+1)/23)>253-(Map.Display.WINDOW_WIDTH/23))
	   {Map.X=23*(253-(Map.Display.WINDOW_WIDTH/23));}
  	   else if(((Map.X+1)/23)<2)
	   {Map.X=46;}  
   
	   if(((Map.Y+1)/23)>(253)-(Map.Display.WINDOW_HEIGHT/23))
	   {Map.Y=23*((253)-(Map.Display.WINDOW_HEIGHT/23));}
  	   else if(((Map.Y+1)/23)<1)
	   {Map.Y=46;}

		return(true);
	}
	/*public string getName(int num)
	{
		return ThingDb.Things.Where(p => p.Value.Map.Objects.Objects.First(obj => obj.objID == num)
	}*/
	public Bitmap GetObjectBitmap(string ObjNum)
	{
		if(ObjNum == null) throw new ArgumentNullException();
		
		return Map.GetObjectBitmap(ObjNum);
	}
	public bool AddObject(string ObjNum, int X, int Y, int callbackID, int DoorFacing)
    {
		if(ObjNum == null) throw new ArgumentNullException("ObjNum");

		return(Map.AddObject(ObjNum,X,Y,callbackID,DoorFacing));
    }	
	public bool DeleteObject(int callbackID)
    {
		return(Map.DeleteObject(callbackID));
    }	
	public bool AddTile(string tileNum, int X, int Y,int variation)
    {
		if(tileNum == null) throw new ArgumentNullException("tileNum");

		return(Map.AddTile(tileNum,X,Y,variation));
    }

	public void ReInit(int x, int y)
    {
	  Map.Display.WINDOW_WIDTH = x;
	  Map.Display.WINDOW_HEIGHT = y;
    }

	public void Render(bool Magnify, Graphics gr)
    {
	 Map.Draw(gr, true,true,true,Map.BGColor);

	 /*if( Magnify )
	    Map.Draw_Magnify(MOUSEX,MOUSEY);*/
	}

    /*public void Update_Window()
	{
      Map.Display.Update_Bounds();
	}*/

	public void InitScreen(Control hWnd)
    {
		win = hWnd;
		Map.Display.Create(0,0,win);
		Map.X = 100;
		Map.Y = 100;
	}

	}
}
