   M S B A M L   `   `   `  ���� F  BZRDLnSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:FoodSafetyMonitoring.ManagerFoodSafetyMonitoring.Manager  6    0FoodSafetyMonitoring.Manager.SysRolePowerManager   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      8x,http://schemas.microsoft.com/winfx/2006/xaml    8src*clr-namespace:FoodSafetyMonitoring.Manager      � 	Resources   �� MergedDictionaries �� Source��  �� $" Themes/ImageButtonStyle.xaml?��� $+ %Themes/GridViewColumnHeaderStyle.xaml?��� $ Themes/TextBlock.xaml?�%    .���  � ColumnDefinitions �� $��* ��� $
��auto �$��6���� $
��1.2* � � RowDefinitions �� $
��0.6* ��� $	��12* ��� $��68 �.+�� 
�������.+��� $	��600"�$��宋体)�$��角色列表��$��10,0,0,0q��� $��1��$��Stretch=�$��Center=�$��6��$��3��� $��2��
�������.+��� $��宋体)�$��权限列表��$	��600"��� -   $��lvlist��$��1��$��0q�$��10,10,10,10q�
������ $��宋体)�$��14(����� ����  �� 
TargetType" ��.���  �� Property � �� �� Value$ 30����  � ��$ 0q���  � ��$ 14(���  � ��$ 宋体)���  � ��
 �   ���  � ��$ Center=� �� View �� .P��� 	 �� Width$	 40��   myHeaderStyle
 �� HeaderContainerStyle#
 [   �� CellTemplate �� .R���  ��Name$ 	imageLogo��$��Images/role.png���� $		 230��$��	角色名��#
 [   �� DisplayMemberBinding �� *	INFO_NAME+�� $		 180��$��角色说明��#
 [   �� *	INFO_EXPL+�� $	 30��#
 [   �� .R��� $��	btnModify��-   $��20��$��20�� ! ListModifyImageButtonTemplate#��[ 
 � Tag �� *	NUMB_ROLE+R� -    R�Name$ tvPermissions��$��1��$��2��$��0q�$��10,10,10,10q�
������ $��宋体)�$��14(�����    �� DataType+   %FoodSafetyMonitoring.Manager.TreeItem �� ItemsSource �� Path �� Orientation d� 	IsChecked �� Mode � ToolTip��%+  (��       *, +)�� "   �� $ children�.R��� $ 
Horizontal=�$��0,2,0,2q�
������ .+��� |�.  $��Center=� �� *	IsChecked+$ TwoWay=��� $��Center=�$��Images/power.png��$��30��$��30���� $��Center=�$��宋体)�$��14(����� *text+ �� *text+�� $��2��$��2��$ 
Horizontal=�$��Center=�$	��Top=�.+��� -   $��btnSave��$��84��$��38��  SaveImageButtonTemplate#��[ $��0,0,20,0q��� -   $��	btnCancel��$��84��$��38��  CancelImageButtonTemplate#��[ $��20,0,0,0q�