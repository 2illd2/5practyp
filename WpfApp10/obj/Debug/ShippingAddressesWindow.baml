   M S B A M L   `   `   `  ����  WpfApp10&     WpfApp10.ShippingAddressesWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  ShippingAddressesWindow��5   	   $	��450��6)   $	��800��66    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%  '��       �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 50��6'   �� 5	       � ��$
 0 10q�6'   �� 5
       � �� �	#FF31C8F96+   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 16(�6)   �� 5       � ��
 �����6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 10��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5"   
   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6   �� 5&      $��* �6   �� 5'      $��* �6   �� 5(      $��* �6   .+�5*   
   &   System.Windows.Controls.DataGrid  -   5        	  Name$	 ShippingAddressesGrid��5*      
   AutoGenerateColumns
 .  $��0��6N   $��6��6[      Columns 5+      0  *System.Windows.Controls.DataGridTextColumn  5,      ,  &System.Windows.Controls.DataGridColumn   Header$ Customer ID��6D   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *
CustomerID+6D     5-      $ Address Line 1��6F    6%   �� *AddressLine1+6F     5.      $
 City��6>    6%   �� *City+6>     5/      $ State��6G    6%   �� *StateShipping+6G     50      $ Postal Code��6D    6%   �� *
PostalCode+6D     51      $ Country��6A    6%   �� *	Country+6A   53      �� 54   
   -   5        $	��Add��54      $	��Top=�67   $��0��6O   $��1��6\   $	��100��6l   $
��Left=�6x   �� 55   
   -   5        $��Update��55      $	��Top=�6=   $��0��6U   $��1��6b   $	��100��6r   $��Center=�6~   �� 56   
   -   5        $��Delete��56      $	��Top=�6=   $��0��6U   $��2��6b   $	��100��6r   $��Right=�6~   �� 57   
   -   5         ��Name$ CustomerComboBox��57      $��1��6-   $��1��6:   $	��200��6J   $��30��6V   ���	#3389B498$
��Left=�6y   �� 58   
   -   5         ��Name$ AddressLine1TextBox��58      $��1��6/   $��1��6<   $	��200��6L   $��30��6X   ���	#3389B498$��Right=�6{   �� 59   
   -   5        $ CityTextBox��59      $��2��6'   $��1��64   $	��200��6D   $��30��6P   ���	#3389B498$
��Left=�6s   �� 5:   
   -   5        $ StateShippingTextBox��5:      $��2��60   $��1��6=   $	��200��6M   $��30��6Y   ���	#3389B498$��Right=�6|   �� 5;   
   -	   5        $ PostalCodeTextBox��5;      $��3��6-   $��1��6:   $	��200��6J   $��30��6V   ���	#3389B498$
��Left=�6y   �� 5<   
   -
   5        $ CountryTextBox��5<      $��3��6*   $��1��67   $	��200��6G   $��30��6S   ���	#3389B498$��Right=�6v   �� 5=   
   $��Customer ID:��6   $��1��6(   $��1��68   $	��Top=�6E   $
��Left=�6]   �� 5>   
   $��Address Line 1:��6   $��1��6+   $��1��6;   $	��Top=�6H   $��Right=�6`   �� 5?   
   $��City:��6   $��2��6!   $��1��6.   $	��Top=�6>   $
��Left=�6V   �� 5@   
   $��State:��6   $��2��6"   $��1��6/   $	��Top=�6?   $��Right=�6W   �� 5A   
   $��Postal Code:��6   $��3��6(   $��1��65   $	��Top=�6E   $
��Left=�6]   �� 5B   
   $��Country:��6   $��3��6$   $��1��61   $	��Top=�6A   $��Right=�6Y   �� 5C   
   -   5        $
��Back��5C      $��1��63   $��5��6C   $	��100��6Q   $��Right=�6]   $��Bottom=�6y   5D      5E      