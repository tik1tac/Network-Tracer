   M S B A M L   `   `   `  ????   Network Tracer%    Network_Tracer.View.LineConnect   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  -     ? 	Resources     	Animation ?? TargetProperty ?? 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O? KeyTime O? Value   EasingFunction.  (System.Windows.Media.Animation.CubicEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode??%?  &	        ?? .??U? $ 	(Line.X1)?$
 line??.\?  $ 0??$ 0Q?  $ 0:0:1??$ 0Q?  $ 0:0:2??$	 800Q?   $ EaseOut=?U? $ 	(Line.X2)?$
 line??.\?  $ 0??$ 0Q?  $ 0:0:1??$	 800Q?   $ EaseIn=?U? $ 	(Line.Y1)?$
 line??.\?  $ 0??$ 0Q?  $ 0:0:1??$ 0Q?  $ 0:0:2??$	 450Q?   $ EaseOut=?U? $ 	(Line.Y2)?$
 line??.\?  $ 0??$ 0Q?  $ 0:0:1??$	 450Q?   $ EaseIn=?.??? -   $??
WireCanvas?? ? ContextMenu ??  ?? 	StaysOpen . .@?m? -   $??Удалить??.+??? -   $
??Line??$??2??
???   ?		 ?? X2$	 40??	
 ?? Y2$
 44??*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ Loaded??*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. <  6Microsoft.Xaml.Behaviors.Media.ControlStoryboardAction  -   a??   ControlStoryboardOption$
 Stop=?5  /Microsoft.Xaml.Behaviors.Media.StoryboardAction   
Storyboard# [  ?? -   $??
ArrowRight??
???   ?$??2???? -   $??	ArrowLeft??
???   ?$??2???? -   $??
WireBorder??$??48??$??48??$y?Hidden=?
???????
????ح?$??3q? ?? CornerRadius$ 30??._??? -	   $??	WireLabel??
???OO/?$??0q?$??Center=?$??Center=? ? Triggers :?  :? RoutedEvent$ Canvas.Loaded??.[??? .???? .??W? $
 Line??$ Opacity? W? From$	 1.0X?	 W? To$	 0.3X? g? Duration$ 0:0:1D? g? AutoReverse .  g? RepeatBehavior$ Forever?? W? EasingFunction   