   M S B A M L   `   `   `  ����C  ?WpfApp10, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WpfApp10WpfApp10  5   	       WpfApp10.ReceiptsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:WpfApp10     5   	     � Title$  ReceiptsWindow��5   	   $	��450��6    $	��800��6-    � 	Resources 5	       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%  '��       �� 5
   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 50��6'   �� 5       � ��$
 0 10q�6'   �� 5       � �� �	#FF31C8F96+   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 16(�6)   �� 5       � ��
 �����6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 10��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5       �  � ColumnDefinitions 5!   
   �� 5"      �� 5#       � RowDefinitions 5&   
   �� 5'      $��* �6   �� 5(      $��* �6   �� 5)      $��* �6   �� 5*      $��* �6   .+�5,   
   &   System.Windows.Controls.DataGrid  -   5        	  Name$	 ReceiptsGrid��5,      
   AutoGenerateColumns
 .  $��0��6E   $��4��6R      Columns 5-      0  *System.Windows.Controls.DataGridTextColumn  5.      ,  &System.Windows.Controls.DataGridColumn   Header$ Order ID��6A   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *	OrderID+6A     5/      $ Receipt Date��6E    6%   �� *ReceiptDate+6E     50      $ Total Amount��6E    6%   �� *TotalAmount+6E     51      $ Change Amount��6F    6%   �� *ChangeAmount+6F     52      $ Amount from Customer��6C    6%   �� *	AmountCus+6C     53      $ Payment Method��6G    6%   �� *PaymentMethod+6G   55      �� 56   
   -   5        $	��Add��56      $	��Top=�67   $��0��6O   $��1��6\   $	��100��6l   $
��Left=�6x   �� 57   
   -   5        $��Update��57      $	��Top=�6=   $��0��6U   $��1��6b   $	��100��6r   $��Center=�6~   �� 58   
   -   5        $��Delete��58      $	��Top=�6=   $��0��6U   $��2��6b   $	��100��6r   $��Right=�6~   �� 59   
   -   5         ��Name$ OrderComboBox��59      $��1��6*   $��1��67   $	��100��6G   $��30��6S   ���	#3389B498$
��Left=�6v   �� 5:   
   -   5         ��Name$ TotalAmountTextBox��5:      $��2��6.   $��1��6;   $	��100��6K   $��30��6W   ���	#3389B498$
��Left=�6{   �� 5;   
   -   5        $ ChangeAmountTextBox��5;      $��2��6/   $��1��6<   $	��100��6L   $��30��6X   ���	#3389B498$��Center=�6|   �� 5<   
   -   5        $ AmountCusTextBox��5<      $��2��6,   $��1��69   $	��100��6I   $��30��6U   ���	#3389B498$��Right=�6y   �� 5=   
   -	   5        $ PaymentMethodTextBox��5=      $��3��60   $��1��6=   $	��200��6M   $��30��6Y   ���	#3389B498$��Center=�6}   �� 5?   
   $��	Order ID:��6   $��1��6%   $��1��65   $	��Top=�6B   $
��Left=�6Z   �� 5@   
   $��Total Amount:��6   $	��Top=�6*   $��1��6B   $��2��6R   $
��Left=�6_   6|   �� 5A   
   $��Change Amount:��6   $	��Top=�6+   $��1��6C   $��2��6S   $��Center=�6`   6   �� 5B   
   $��Amount from Customer:��6   $	��Top=�62   $��1��6J   $��2��6Z   $��Right=�6g   6�   �� 5C   
   $��Payment Method:��6   $	��Top=�6,   $��1��6D   $��3��6T   $
��Left=�6a   6~   �� 5D   
   -
   5        $
��Back��5D      $��1��63   $��3��6C   $	��100��6Q   $��Right=�6]   $��Bottom=�6y   5E      5F      