   M S B A M L   `   `   `  ����>  :Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WpfWpf  5   	       Wpf.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:Wpf     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ taskGrid��5       $��0��6      AutoGenerateColumns$ False��62      CanUserAddRows$ False��6N   ��5      �� *
TaskList+ �� SelectedItem 64   �� *SelectedTask+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *TaskID+  5      $ Title��6%    64   �� *	TaskTitle+  5      $ Description��6%    6:   �� *TaskDescription+  5      $ Done or undone ��6%    6>   �� *
TaskDone+5      �� 5"   
   $��1��6   	 �� Orientation$	 
Horizontal=�6"   $��Right=�6;   $��
0,10,10,10q�6W   .+�5#      �� -   5        $
��Edit��5#      $��0,0,5,0q�6$   65   �� 5$      -   5        $	��Add��5$      $��0,0,5,0q�6#   64   �� 5%      -   5        $��Delete��5%      6&   5&      �� 5'   
   $��1��6   $	 
Horizontal=�6"   $
��Left=�6;   $��
0,10,10,10q�6V   .+�5(      �� -   5        $��How to do it��5(      $��0,0,5,0q�6,   6=   5)      5*      5+      