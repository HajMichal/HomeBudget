   M S B A M L   `   `   `  ���� 
  WpfApp    WpfApp.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
MainWindow��$	��720��$
��1280��.���  � ColumnDefinitions �� $��1* ��� $��3* �.+��� $��0��$��10q�.+��� $��LISTA BUDŻETÓW��$��20(�$��0,0,0,20q��� $��10q� �� Padding$ 10q� �� CornerRadius$ 10��$��2q�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$
 Blue�� � Offset$	 0.0Q�� $ Green��$	 1.0Q�._��� .+��� �� $�
Wypłata: ��$
��Bold"� �� -    �Name$ Salary���� �� $�	Wydatki: ��$
��Bold"� �� -   $ Expenses���� �� $�Saldo: ��$
��Bold"� �� -   $ Balance���� $��10q�$ 10q�$ 10��$��2q�$��0������ $	 0,0&�$	 1,1&�.��� $
 Blue��$	 0.0Q�� $ Green��$	 1.0Q�._��� -   $��BudgetListBox��$	��200��$��0,0,0,0q�$��0q��� -   $��BtnCreateBudget��$��Stwórz budżet��$	��0,5q��� -   $��BtnRemoveBudget��$��Usuń budżet��$	��0,5q��� -   $��BtnShowBudget��$��Wyświetl statystyki��$	��0,5q��� -   $��CreateTransactionPanel��$y�	Collapsed=�$��	0,20,0,20q�.+��� -	   $��BtnAddTransaction��$��Dodaj transakcję���� -
   $��ModifyTransactionPanel��$y�	Collapsed=�$��0,20,0,0q�.+��� -   $��BtnRmvTransaction��$��Usuń transakcję��
���  ��$	��0,5q��� $��1��$��10q�.+��� -   	 ��Name$	 
StatsField��
����+��$��1q�$ 10q�$y�	Collapsed=�._��  �� $��* ��� $��* �.+��� -   $��TransactionsListBox��$��0��$	��600��$
��0,10q��� -   $��TransactionDetailsListBox��$��1��$	��600��$
��0,10q�$y�	Collapsed=��� -   $	 
BudgetForm��
����  �$��1q�$ 10q�$y�	Collapsed=�._��� .+��� $��Nazwa:���� -   $��
BudgetName��$��0,0,0,10q��� $��Zarobek:���� -   $��MonthSalary��$��0,0,0,10q��� -   $��Stwórz Budżet��$��0,10,0,0q��� -   $	 TransactionForm��
��� � �$��1q�$ 10q�$y�	Collapsed=�._��� .+��� $��Nazwa:���� -   $��TransactionName��$��0,0,0,10q��� $��
Kategoria:���� -   $��TransactionCategory��
 �� SelectedIndex$
 0���� $+��%Data (opcjonalnie) Format: dd/MM/yyyy���� -   $��TransactionDate��$��0,0,0,10q���  �� Orientation$ 
Horizontal=�$��0,20,0,0q�.+��� Nazwa:�� -   $��ProductName��$	��150��$��0,0,10,0q��� Cena:�� -   $��ProductPrice��$	��100��$��0,0,10,0q��� 
Ilość:�� -   $��ProductQuantity��$��50��$��0,0,10,0q��� -   $��dodaj��$��0,0,10,0q��� -   $
��usun��$��0,0,10,0q��� -   $��Stwórz Transakcje��$��0,10,0,0q��� -   $��ProductListBox��
�������$	��100��$
��0,10q�