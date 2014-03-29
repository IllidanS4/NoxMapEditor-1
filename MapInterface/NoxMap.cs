/* Date: 1.3.2014, Time: 13:43 */
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NoxShared;

namespace MapInterface
{
	public class NOX_MAP
	{
	  public int X;
	  public int Y;
	  public int MOUSE_X;
	  public int MOUSE_Y;
	  public Color BGColor;

	  //Callbacks
	  //typedef void (__stdcall *ObjMove_CALLBACK)(int ObjNum,int X,int Y);
	  //ObjMove_CALLBACK ObjMoveFunc; 
	 // public delegate void OBJMoved_CALLBACK(int ObjNum,int X,int Y);
	  //The Display
      public VideoBag Video;
      public GUI_DISPLAY Display = new GUI_DISPLAY();
      
	  //The Map Class'
	  public OBJECT_MAP Objects = new OBJECT_MAP();
	  public FLOOR_MAP Floor = new FLOOR_MAP();

	  //Drawing
	  public int MAG_MAX;
	  public int MAG_SIZE;
	  public bool scaleOn;
	  
	  public void SetMouse(int X, int Y)
	  {
		MOUSE_X = X;
		MOUSE_Y = Y;
	  }


      public NOX_MAP(string noxdir)
      {
	    scaleOn = false;
	    
	    Video = new VideoBag(noxdir+"/video.bag");
		/*Video.Init();
		Video.idx.LoadIdx();
		Thing.Load_Thingdb(Video.thingpath);*/
		X=0;
		Y=0;
		BGColor = Color.Empty;
	    MAG_MAX = 50;
	    MAG_SIZE = 100;
      }
      
/*public void Draw_Magnify(int x,int y)
{

	if(x < 0 || x > Display.WINDOW_WIDTH || y < 0 || y > Display.WINDOW_HEIGHT)
		return;


	int mod=MAG_MAX;
	if(x<MAG_MAX)
       mod=x;
	if(y<MAG_MAX)
	   mod=y;


	if(x>Display.WINDOW_WIDTH-MAG_MAX)
       mod=Display.WINDOW_WIDTH-x;
	if(y>Display.WINDOW_HEIGHT-MAG_MAX)
	   mod=Display.WINDOW_HEIGHT-y;


	Rectangle prc = new Rectangle(x-mod, y-mod, 2*mod, 2*mod);

	var bb = Display.Display.GetBackBuffer();
	var col = Color.FromArgb(155, 155, 155);
	Display.Display.Blt(0,0,MAG_SIZE,MAG_SIZE,bb,&prc); 
    Display.Display.ColorFill(MAG_SIZE,0,2,MAG_SIZE+2,Display.Display.GetBackBuffer(),col);
    Display.Display.ColorFill(0,MAG_SIZE,MAG_SIZE+2,2,Display.Display.GetBackBuffer(),col);
}*/
public bool AddTile(string objName, int X, int Y, int variation)
{
	if( X < 0 || X > 255 ||
		Y < 0 || Y > 255)
		return(false);
	int ObjNum=0;
	ThingDb.Tile tile = tile = ThingDb.FloorTiles.Find(t => t.Name == objName);

	if( tile == null )
		return false;
	VideoBag.Section.SectionEntry Entry = null;
	VideoBag.Section Section = null;
	Bitmap surf;
    SURFACE tem = new SURFACE();
    SURFACE test;
	
	int val = (int)tile.Variations[variation];
		val++;

test = Floor.TileImgs.Find(img => img.Image_ID == val);
if( test == null )
{
	surf = null;
    Bitmap img = Video.ExtractOne(val, out Section, out Entry);
		
		if( img != null )
		{
			tem.surface = img;
			tem.Height = img.Height;
			tem.Width = img.Width;
			tem.Image_ID = val;
			tem.ImgType = Entry.type;
			tem.OffsetX = 0/*Entry.OffsetX*/;
			tem.OffsetY = 0/*Entry.OffsetY*/;
		}


		//LOAD THE NAME AS WELL NOW!!!!!
		Floor.TileImgs.Add(tem);    
		Floor.tileMap[X,Y].pTile = tem;
}
else
{
  Floor.tileMap[X,Y].pTile = test;
}
		Floor.tileMap[X,Y].Tile = ObjNum;
		Floor.tileMap[X,Y].tVari = variation;
		Floor.tileMap[X,Y].TileImg = val;
	
		tem.surface = null;
		surf = null;


return(true);
}
public void Draw(Graphics gr, bool DrawFloor, bool DrawWalls, bool DrawObjects, Color FloorColor)
{
	gr.Clear(FloorColor);
int TileSize = 23; //23
int Subtractor = 255; // 255
int FullTileSize = 46; // 45

int/*int*/ xoff = X%TileSize;	
int /*int*/ yoff = Y%TileSize;
double /*int*/ x = X/TileSize;
double /*int*/ y = Y/TileSize;

if( x < 0 )
   x = 0;
else if (x >(Subtractor - Display.WINDOW_WIDTH/TileSize))
   x = Subtractor - Display.WINDOW_WIDTH/TileSize;		

if( y < 0 )
   y = 0;
else if (y > (Subtractor - Display.WINDOW_HEIGHT/TileSize))
   y = Subtractor - Display.WINDOW_HEIGHT/TileSize;

	for(int i=(0),i2=(-1); i<(Display.WINDOW_HEIGHT/TileSize)+3/*Overlap so pretty edges*/;  i++,i2++)
	{
       for(int j=(0),j2=(-1); j<(Display.WINDOW_WIDTH/TileSize)+3/*Overlap*/; j++,j2++)
	   {
          if(Floor.tileMap[i+Y/TileSize,j+X/TileSize].Tile != (-1))
		  {       
			 SURFACE tem = Floor.tileMap[i+Y/TileSize,j+X/TileSize].pTile; //=  Floor.TileImgs.Find(Floor.tileMap[i+Y/TileSize][j+X/TileSize].TileImg);
			 if( tem != null && tem.surface  != null)
			 	gr.DrawImage(tem.surface, ((j2)*TileSize)-xoff,((i2)*TileSize)-yoff);
	      }
	   }
	}

	OBJECT obj = null;
	xoff = Display.WINDOW_WIDTH;
	yoff = Display.WINDOW_HEIGHT;

	for( int i=0; i<Objects.Objects.Count; i++)
	{
		obj = Objects.Objects[i];
		if( obj == null )
		{
			Objects.Objects.RemoveAt(i);
			i--;
			continue;
		}
		if( (obj.X + obj.Width) > X && (obj.X) < X+xoff &&
            (obj.Y + obj.Height) > Y && (obj.Y) < Y+yoff)
		{
			// Use the objects extents to display it
			//Thing.Thing.Object.Objects.Get(obj.objID);
			SURFACE tem = Objects.ObjImages.Find(s => s.Image_ID == obj.imgID);
			int xs = obj.Xlen;
			int ys = obj.Ylen;

			if( xs > 0 && obj.ExtentBox)
				xs /= 2;
			if( ys > 0 && obj.ExtentBox)
				ys /= 2;


			 if( tem != null && tem.surface != null)
			 {
				 if( obj.ExtentBox )
				 	gr.DrawImage(tem.surface, (((obj.X-X)-obj.Width/2)-(xs)),((((obj.Y-Y)-obj.Height)-(ys))));
				 else if(obj.ExtentCircle)
				 	gr.DrawImage(tem.surface, (((obj.X-X)-obj.Width/2)-xs),((((obj.Y-Y))-obj.Height)-xs));
				 else
				 	gr.DrawImage(tem.surface, ((obj.X-X)-/*tem.OffsetX*/obj.Width),((obj.Y-Y)-/*tem.OffsetY*/obj.Height));
			 }		
		}
	}
	//Objects.Objects.ClearGet();

//Display.Draw_Text("Nox Graphical Map Editor Interface",
//				  RGB(0,0,0),RGB(120,120,120),0,0);
//Display.Draw_Text("Alpha 1.5",
//				  RGB(0,0,0),RGB(120,120,120),0,15);
//Display.Draw_Text("April 17 2007",
//				  RGB(0,0,0),RGB(120,120,120),0,30);

if( scaleOn )
{
	/*x = ScaleSurface.Description.Width;
		y= ScaleSurface.Description.Height;*/
		throw new NotImplementedException();//TODO
	//Display.Display.Blt(MOUSE_X-x/2,MOUSE_Y-y/2,x,y,ScaleSurface.GetDDrawSurface(),null,DDBLT_KEYSRC);
}

}
/*bool SaveWallBMP(char wallName[], char fileName[]) 
{
	int WallNum =0;
	LLAW * wall;
	for(wall = Thing.Thing.Wall.Walls.Get(); wall && strcmp(((char*)&wall.Name),wallName); wall = Thing.Thing.Wall.Walls.Get(), WallNum++);
	Thing.Thing.Wall.Walls.ClearGet();

	//OBJECT object;
    //SURFACE tem;
    //SURFACE * test = null;

	if( !wall )
      return(false);

	IDX_DB::Entry * Entry = null;
	IDX_DB::SectionHeader * Section = null;
    unsigned int val = 0;

	//wall.ImageCode
	if(wall.Images.numNodes > 0)	
		val = *wall.Images.Get(0);
          
		  if( !val || val < 0 )	
			  return(false);
	
		  Video.idx.GetAll(val,&Entry,&Section);
		  if( !Entry || !Section)
			  return false;

		  fstream file;	
		  file.open(Video.bagpath,ios::in | ios::binary);

		  Video.Extract(&file,Section,Entry,fileName//*"TESTMAP.bmp");
		  file.close();
return(true);
}*/
public Bitmap GetObjectBitmap(string objName) 
{
	return Video.ExtractOne(ThingDb.Things[objName].PrettyImage);
}
public bool LoadObject(string objName, ref Bitmap surface)
{
	int ObjNum =0;
	/*GNHT * obj;
	for(obj = Thing.Thing.Object.Objects.Get(); obj && strcmp(obj.Name,objName); obj = Thing.Thing.Object.Objects.Get(), ObjNum++);
	Thing.Thing.Object.Objects.ClearGet();*/
	ThingDb.Thing obj = ThingDb.Things[objName];

	OBJECT @object;
    SURFACE tem;
    SURFACE test = null;

	if( obj == null )
      return(false);

    int val = 0;


		 // else if(obj.Stats.numNodes > 0 && obj.Stats.numNodes<2)
           //   val = *obj.Stats.Get(0).Images.Get(0);

		  if(obj.States.Count > 0)
		  	val = obj.States[0].Animation.Frames[0];
		  else if(obj.PrettyImage!=0)
			  val = obj.PrettyImage;
		  else if(obj.MenuIcon!=0)
			  val = obj.MenuIcon;	
		  else
			  return(false);
	      if( val != 0 )
			  val++;
		  else
			  return(false);
          
		  if( val <= 0 )
			  return(false);

          //test = Objects.ObjImages.Find(val);
          if( val != 0 && (test = Objects.ObjImages.Find(s => s.Image_ID == val)) != null )
		  {

          surface = Video.ExtractOne(val);
          
		  /*Video.idx.GetAll((unsigned int)val,&Entry,&Section);
		  fstream file;	
		  file.open(Video.bagpath,ios::in | ios::binary);

				  NxImage * img = Video.Extract(&file,Section,Entry,null);
			 // Display.Display.CreateSurfaceFromBitmap(surface,"TESTMAP.bmp");
			  Display.Display.CreateSurface(surface,img.width,img.height);
			  Display.BltBitmapData(img.data, img.width, img.height, (*surface).GetDDrawSurface());
			  //img.DrawToSurface(0,0,*surface,&Display.pixF,true);
			  delete img;
			  if( *surface )
			      (*surface).SetColorKey(RGB(248,7,248));
			  //remove("TESTMAP.bmp");*/
		  }
return(true);
}
public bool AddObject(string objName, int X, int Y, int callback, int DoorFacing)
{
	/*RGBQUAD test;
	test.rgbBlue = 255;
	test.rgbRed = 255;
	Video.bmpfile.Set_Color(test);
*/
	int ObjNum =0;
	/*GNHT * obj;
	for(obj = Thing.Thing.Object.Objects.Get(); obj && strcmp(obj.Name,objName); obj = Thing.Thing.Object.Objects.Get(), ObjNum++);
	Thing.Thing.Object.Objects.ClearGet();*/
	var obj = ThingDb.Things[objName];
	OBJECT @object = new OBJECT();


    Bitmap surf = null;
    SURFACE tem = new SURFACE();
    SURFACE test = null;

	if( obj == null )
      return(false);

    int val = 0;


		 // else if(obj.Stats.numNodes > 0 && obj.Stats.numNodes<2)
           //   val = *obj.Stats.Get(0).Images.Get(0);
	if( DoorFacing == -1)
	{
		  if(obj.States.Count > 0)
		  	val = obj.States[0].Animation.Frames[0];
		  else if(obj.PrettyImage!=null)
			  val = obj.PrettyImage;
		  else if(obj.MenuIcon!=null)
			  val = obj.MenuIcon;	
		  else
			  return(false);
	      if( val != 0 )
			  val++;
		  else
			  return(false);
          
	}
	else
	{
		try{
			
		switch(DoorFacing)
		{
			case 0x00: // South
				val = obj.States[0].Animation.Frames[0];
				break;
			case 0x08: // West
				val = obj.States[0].Animation.Frames[8];
				break;
			case 0x10: // North
				val = obj.States[0].Animation.Frames[16];
				break;
			case 0x18: // East
				val = obj.States[0].Animation.Frames[24];
				break;

			default:break;
		}
		  if( val != 0 )
			  val++;
		  else
			  return(false);
		}catch(ArgumentOutOfRangeException)
		{
			
		}
	}


		  if( val <= 0 )
			  return(false);

          var img = Video.ExtractOne(val);
          

	if( img != null && img.Height >0 && img.Width >0)
	{
		surf = img;
	}	
			tem.surface =surf;
			tem.Height = img.Height;
			tem.Width = img.Width;
			tem.Image_ID = val;
			//tem.ImgType = @object.;
			tem.OffsetX = X;
			tem.OffsetY = Y;
			
			//LOAD THE NAME AS WELL NOW!!!!!
			Objects.ObjImages.Add(tem);
			//test = tem.ms;
	
		 @object.imgID = val;
		 @object.objID = ObjNum;
		 @object.X = X;
		 @object.Y = Y;
		 @object.callbackID = callback;
		 @object.Xlen = obj.ExtentX;
		 @object.Ylen = obj.ExtentY;
		 @object.Z = obj.Z;
		 @object.zSizeX = obj.ZSizeX;
		 @object.zSizeY = obj.ZSizeY;
		 @object.DoorFacing = DoorFacing;
		 //object.ExtentBox = obj.Properties;
		 @object.ExtentBox = false;
		 @object.ExtentCircle = false;

		 /*Property *prop;
		 prop = obj.Properties.Get();
		 while( prop != null)
		 {
			char * st = strstr((char*)prop.Value,"EXTENT");
			if( st != null)
			{
				st+=9;
				if(strstr(st,"CIRCLE") != null)
				{
					st += 7;
					int val = atoi(st);
					@object.ExtentCircle = true;
					@object.Xlen = val;
				
					//is circle
					// read 1 #
				}
				else if(strstr(st,"BOX") != null)
				{
					st += 4;
					int val = atoi(st);
					while( *st != ' ')
						st++;
					st++;
					int val2 = atoi(st);
					@object.ExtentBox = true;
					@object.Xlen = val;
					@object.Ylen = val2;
					//is box
					// read 2 #'s
				}
			}
			st = strstr((char*)prop.Value,"Z");
			if( st != null)
			{
				st += 4;
				obj.Z = atoi(st);
			}
			st = strstr((char*)prop.Value,"ZSIZE");
			if( st != null)
			{
				st += 8;
				int val = atoi(st);
				while( *st != ' ')
					st++;
				st++;
				int val2 = atoi(st);
				obj.ZSizeX = val;
				obj.ZSizeY = val2;
			}	
			st = strstr((char*)prop.Value,"SIZE");
			if( st != null)
			{
				
			}	
			prop = obj.Properties.Get();
		 }
		 obj.Properties.ClearGet();*/
		
		if(test != null)
		 {
			@object.Height = test.Height;
			@object.Width = test.Width;
		 }
		 Objects.Objects.Add(@object);
	
	tem.surface = null;
    surf = null;
	/*test.rgbBlue = 0;
	test.rgbRed = 0;
	Video.bmpfile.Set_Color(test);
*/
return(true);
}

public bool DeleteObject(int callback)
{
	return Objects.Objects.RemoveAll(obj => obj.callbackID == callback) > 0;
}
/*public bool Scale(bool on)
{
 if( ScaleSurface != null )
 {
	ScaleSurface.ReleaseGraphics();
	ScaleSurface = null;
 }
 scaleOn = on;
 if( on )
	LoadObject("NPC", ref ScaleSurface);
return(true);
}*/
	}
}
