   M S B A M L   `   `   `  ????  Network Tracer%    Network_Tracer.View.LineConnect   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      -   5          ? 	Resources  5	         	Animation ?? TargetProperty ?? 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O? KeyTime O? Value   EasingFunction.  (System.Windows.Media.Animation.CubicEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode??%?  &	        ?? 5
   
   .??5      U? $ 	(Line.X1)?6,   $
 line??6R   .\?5        $ 0??6'   $ 0Q?63     5      $ 0:0:1??6'   $ 0Q?67     5      $ 0:0:2??6'   $	 800Q?67    5        5      $ EaseOut=?6$   5      5      U? 5      $ 	(Line.X2)?6,   $
 line??6R   .\?5        $ 0??6'   $ 0Q?63     5      $ 0:0:1??6'   $	 800Q?67    5        5      $ EaseIn=?6$   5      5      U? 5      $ 	(Line.Y1)?6,   $
 line??6R   .\?5        $ 0??6'   $ 0Q?63     5       $ 0:0:1??6'   $ 0Q?67     5!      $ 0:0:2??6'   $	 450Q?67    5"        5#      $ EaseOut=?6$   5%      5&      U? 5(      $ 	(Line.Y2)?6,   $
 line??6R   .\?5)        $ 0??6'   $ 0Q?63     5*      $ 0:0:1??6'   $	 450Q?67    5+        5,      $ EaseIn=?6$   5.      5/      50      51      .??53      ? -   5        $??
WireCanvas??53       ? ContextMenu 54   
   ?? 55       ?? 	StaysOpen . .@?56      m? -   5        $??Удалить??56      57      58      .+?5;   
   ?? -   5        $
??Line??5;      $??2??6   
???   ?		 ?? X2$	 40??6>   	
 ?? Y2$
 44??6F   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5<      +  %Microsoft.Xaml.Behaviors.EventTrigger  5=         	EventName$ Loaded??6!   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5>      <  6Microsoft.Xaml.Behaviors.Media.ControlStoryboardAction  -   5        a??   ControlStoryboardOption$
 Stop=?5>   ;   5  /Microsoft.Xaml.Behaviors.Media.StoryboardAction   
Storyboard# [  6Z   5?      5A      ?? 5B   
   -   5        $??
ArrowRight??5B      
???   ?$??2??61   ?? 5C   
   -   5        $??	ArrowLeft??5C      
???   ?$??2??60   ?? 5D   
   -   5        $??
WireBorder??5D      $??48??6#   $??48??6.   $y?Hidden=?6:   
???????
????ح?$??3q?6~    ?? CornerRadius$ 30??6?   ._?5E      ?? -	   5        $??	WireLabel??5E      
???OO/?$??0q?6@   $??Center=?6L   $??Center=?6i   6?   5F       ? Triggers 5G   
   :? 5H       :? RoutedEvent$ Canvas.Loaded??6   .[?5I      ?? .??5J      ?? .??5K      W? $
 Line??5L      $ Opacity?5M       W? From$	 1.0X?5N      	 W? To$	 0.3X?6(    g? Duration$ 0:0:1D?61    g? AutoReverse .  g? RepeatBehavior$ Forever??5O   0    W? EasingFunction 5P        5Q   "   6.   5S      5T      5U      5V      5X      5Y      