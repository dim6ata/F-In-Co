   M S B A M L   `   `   `  ����J  FENTERPRISE_CWK2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ENTERPRISE_CWK2.ViewControllersENTERPRISE_CWK2.ViewControllers  5      A%clr-namespace:ENTERPRISE_CWK2.UtilityENTERPRISE_CWK2.Utility  5      2    ,ENTERPRISE_CWK2.ViewControllers.AccountsCtrl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:ENTERPRISE_CWK2.ViewControllers     5      8custom%clr-namespace:ENTERPRISE_CWK2.Utility     5         userControlStyle#��[  5	      .��5      ]� .+�5   
   � $	��Top=�6     � ColumnDefinitions  5      �� 5      �� 5      �� 5       � RowDefinitions 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       .+�5%      (� $��0��6   $��0��6&   $
��Left=�63   .��5&      ��   largeLabelStyle#��[ 6   .��6B   
Accounts6L   5'      (� 5)      $��1��6   $��2��6&   $��0��6:   $��Right=�6G   .��5*      �� -   5         ��Name$ accButtonsStackPanel��5*       �� Orientation$ 
Horizontal=�6;   5,      5-      �� 50      $��0��6   $��1��6%   $��3��62   $��2��6F    �� Padding$ 5,0,5,0q�6W   ._�51      (� $
��Left=�6   .��54      �� -   5        $ accountsHolderStackPanel��54   !   $ 
Horizontal=�6C   5;      5=      5>      � 5@      $��0��6   $��3��6#     5A      �� 5B      �� 5C      .+�5F      (� $��0��6   $��0��6*   $
��Left=�67   .��5H      �� -   5        $��transactionLabel��5H       $y�Hidden=�68     subSectionLabelStyle#��[ 6L   .��6{   Transactions:6�   5J      �� 5L      $��0��6   $��1��6&   $
��Left=�66   $ 15q�6Q   $��Center=�6^   ._�5M      (� .��5N      �� -   5        $��transactionsComboBox��5N      $��30��6;   $	��100��6G   $��Center=�5O      $��Center=�6C   $��10(�5P      $y�Hidden=�5Q   L    �� SelectedValuePath$ Content��5R        
GreenColor#��[ 67     CustomComboBoxStyle#���  5S      .@�5T      �� -   5         ��Name$ viewAllItem��5T   '    �� 
IsSelected . .��6N   
View All6X   �� 5U      -   5        $ viewIncomeItem��5U   '   .��6?   View Income6L   �� 5V      -   5        $ viewExpenseItem��5V   '   .��6@   View Expenses6O   5W      5X      5Y      5[      �� 5]      -   5        $! transactionButtonStackPanel��5]      $ 
Horizontal=�6>   $��Center=�6W   $��3��6r   $��1��6   $��2��6�   5_      &   System.Windows.Controls.DataGrid  5t      -	   5          Name$ accountDataGrid��5u      $��4��5t      $��6��6,   $��3��6=   $��	10,0,10,0q�6Q   $y�Hidden=�5u   0     dataGridStyle#��[ 5w      	   Columns	 5y      0  *System.Windows.Controls.DataGridTextColumn  5{      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Type��61      Width5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6b   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   ��  �� Path$ 	TransType�6b     5|      $

 Name��6)   $ 2* 6i    67   �� $ TransContact.ContactName�6i     5}      $

 Date��6)   $ * 6j    67   �� $ 	TransDate� �� StringFormat$ d��6j     5~      $
 Account��6)   $ * 6n    6:   �� $  TransContact.ContactAccNum�6n     5      $
 	Reference��6)   $ 2* 6d    6<   �� $ TransReference�6d     5�      $
 Amount��6)   $ * 6d    69   �� $ TransAmountString�6d   5�      5�      5�      5�      