   M S B A M L   `   `   `  ����  WpfApp10     WpfApp10.IngredientsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  IngredientsWindow��5   	   $	��450��6#   $	��800��60    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%  '��       �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 50��6'   �� 5	       � ��$
 0 10q�6'   �� 5
       � �� �	#FF31C8F96+   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 16(�6)   �� 5       � ��
 �����6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 10��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5!   
   �� 5"      $��* �6   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6   .+�5'   
   &   System.Windows.Controls.DataGrid  -   5        	  Name$	 IngredientsGrid��5'      
   AutoGenerateColumns
 .  $��0��6H      Columns 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *NameIngredient+6H     5*      $ Description��6O    6%   �� *DescriptionIngredient+6O   5,      �� 5-   
   -   5        $	��Add��5-      $	��Top=�67   $��0��6O   $��1��6\   $	��100��6l   $��Right=�6x   �� 5.   
   -   5        $��Изменить��5.      $	��Top=�6?   $��0��6W   $��1��6d   $	��100��6t   $
��Left=�6�   �� 5/   
   -   5        $��Delete��5/      $	��Top=�6=   $��0��6U   $��2��6b   $	��100��6r   $��Center=�6~   �� 50   
   -   5         ��Name$ NameIngredientTextBox��50      $��1��61   $��1��6>   $	��200��6N   $��30��6Z   $��Center=�6f   $��Center=�6�   $��5q�6�   �� 51   
   -   5        $" DescriptionIngredientTextBox��51      $��2��68   $��1��6E   $	��200��6U   $��30��6a   $��Center=�6m   $��Center=�6�   $��5q�6�   �� 52   
   $��Name:��6   $��1��6!   $��1��6.   $��Center=�6>   $
��Left=�6Y   �� 53   
   $��Description:��6   $��2��6(   $��1��65   $��Center=�6E   $
��Left=�6`   �� 54   
   -   5        $
��Back��54      $��1��63   $��3��6C   $	��100��6Q   $��Right=�6]   $��Bottom=�6y   55      56      