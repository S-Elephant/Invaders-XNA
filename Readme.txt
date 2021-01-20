==============================================================================
Installation Instructions
==============================================================================
Requires the .NET framework 4.0:
http://www.microsoft.com/download/en/details.aspx?id=17851
And XNA 4.0:
http://www.microsoft.com/download/en/details.aspx?id=20914 (6.7mb)

If you have those two above installed then you can just run the game by
starting "Invaders.exe".

==============================================================================
About
==============================================================================
A prototype space invaders game written in C#/XNA.
Uses the XNALib DLL file: https://sourceforge.net/projects/xnalibrary/

==============================================================================
Manual
==============================================================================
All players:
[M]       : cycles through the background musics
[Escape]  : Returns to the main menu or exits the game.

Player 1:
[A]       : Move left
[D]       : Move right
[Space]   : Fire

Player 2:
[Numpad4] : Move left
[Numpad6] : Move right
[Numpad0] : Fire

Lives are shared between players. If you get hit you lose a life. If you have
no more lives remaining then your ship is destroyed. If all player ships have
been destroyed then it's Game Over.

==============================================================================
Legal Note
==============================================================================
The music tracks included are not part of the open source license but remain
property of Aj Lornie. Ask him for the copyrights on these tracks. I have
permission from him to use them for my games.

==============================================================================
Version History
==============================================================================

Version 1.0.3 (April 14 2012)
  - Rewrote the readme file.
  - Updated the project for the new XNALib DLL
  - Added a lot of comments
  - Now FPS independent


Release 2 (August 12 2011):
  - Added 2 music tracks
  - Extended the XactMgr library with volume levels 
  - Changed controls
  - Added Readme
  - Fixed a bug when the user pressed [Escape] in the Mainmenu
  - Fixed a bug that prevented the bonus enemy from being disposed so that
    the next level would not load when this enemy would not have been
	destroyed.
  - New Controls:
Change music: [M]
		Player 1:
			Left:  [A]
			Right: [D]
			Shoot: [Space]
		Player 2:
			Left:  [Numpad 4]
			Right: [Numpad 6]
			Shoot: [Numpad 0]


Release 1 (August 11 2011):
  - Initial release.