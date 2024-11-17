set target_dir=..\..\client\Assets\Packages\com.game.framework.bsk.core\Plugins\


copy  .\bin\Output\netstandard2.1\game.core.dll			        %target_dir%\ /Y
copy  .\bin\Output\netstandard2.1\game.core.pdb			        %target_dir%\ /Y
copy  .\bin\Output\netstandard2.1\Game.Math.dll			        %target_dir%\ /Y
copy  .\bin\Output\netstandard2.1\Game.Math.pdb			        %target_dir%\ /Y
pause