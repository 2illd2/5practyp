   M S B A M L   `   `   `  ����  WpfApp10'    !WpfApp10.ProductIngredientsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  ProductIngredientsWindow��5   	   $	��450��6*   $	��800��67    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%  '��       �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 50��6'   �� 5	       � ��$
 0 10q�6'   �� 5
       � �� �	#FF31C8F96+   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 16(�6)   �� 5       � ��
 �����6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 10��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5"   
   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6   �� 5&      $��* �6   .+�5(   
   &   System.Windows.Controls.DataGrid  -   5        	  Name$	 ProductIngredientsGrid��5(      $��1��63   $��0��6G   
   AutoGenerateColumns
 .     Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Product ID��6C   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *	ProductID+6C     5+      $ Ingredient ID��6F    6%   �� *IngredientID+6F   5-      �� 5.   
   $��Product ID:��6   $��1��6'   $��1��67   $	��Top=�6D   $
��Left=�6\   �� 5/   
   -   5         ��Name$ ProductComboBox��5/      $��1��6,   $��1��69   $	��200��6J   $��30��6V   $��5q�6b    �� DisplayMemberPath$ ProductName��6m    �� SelectedValuePath$ 	ProductID��6�   �� 50   
   $��Ingredientt ID:��6   $��1��6+   $��2��6;   $	��Top=�6H   $
��Left=�6`   �� 51   
   -   5        $ IngredientComboBox��51      $��2��6/   $��1��6<   $	��200��6M   $��30��6Y   $��5q�6e   $ NameIngredient��6p   $ IngredientID��6�   �� 53   
   -   5        $	��Add��53      $	��Top=�67   $��0��6O   $��1��6\   $	��100��6l   $
��Left=�6x   �� 54   
   -   5        $��Update��54      $	��Top=�6=   $��0��6U   $��1��6b   $	��100��6r   $��Center=�6~   �� 55   
   -   5        $��Delete��55      $	��Top=�6=   $��0��6U   $��2��6b   $	��100��6r   $��Right=�6~   �� 56   
   -   5        $
��Back��56      $��1��63   $��3��6C   $	��100��6Q   $��Right=�6]   $��Bottom=�6y   57      58      