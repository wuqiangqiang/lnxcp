   M S B A M L   `   `   `  ����  检测监管系统.    (FoodSafetyMonitoring.Manager.SetSampling   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   $	��510��5   	   $	��463��6     � WindowStyle$
  None=�6"    � WindowStartupLocation$ CenterScreen=�65   
������  � 
ResizeMode$ NoResize=�6t   
������  � AllowsTransparency .  � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Themes/TextBlock.xaml?�6%   �� 5	      $ Themes/Combobox.xaml?�6%   �� 5
      $ Themes/TextBoxStyle.xaml?�6%   �� 5      $" Themes/ImageButtonStyle.xaml?�6%   %    5      .��5      � ��5   
   �� 5       �� ImageSource$ ../res/sampling.png��6    � RowDefinitions 5   
   �� 5      $��55 �6   �� 5      �� 5      $	��120 �6   .+�5   
   � $��
13,11,11,0q�6   .+�5      n� -   5        $	��440��5      $��40��6    
������ 	 J� Opacity$	 0Q�6E   $
��Left=�6m   6�   �� 5      $��20��6   $	��160��6$   $
��Left=�60   $��16(�6K   $��Center=�6Y   
�������抽检率设置6�   �� 5      -   5        
 ��Name$

 exit��5   D   $��29��6   $��26��6    $��/res/close.png��6,    � ToolTip$ 关闭��6�   $��Right=�6�   $	��Top=�6�   5      � 5   
   -   5         �Name$ _grid_detail��5      $��
13,50,13,0q�6%   $	��Top=�69   $��Center=�6Q   $��1��6n   5      � 5    
   $��2��6   $��
13,0,13,50q�6   .+�5!      �� -   5        $��btnSave��5!      $	��120��6$   $��32��60      SaveImageButtonTemplate#��[  6<   6m   5"      5#      5$      