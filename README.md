## Описание
Проектирование, программирование и администрирование базы данных в MS SQL Server и разработка интерфейса в Windows Forms для управления базой данных для отдела продаж на заводе Ашасветотехника. 

## 1. Создадим базу данных в SQL Server  
### 1. Создадим и определим таблицы, зададим первичные ключи  
  •	Таблица счетов – accounts   
  •	Таблица названий категорий техники – category names  
  •	Таблица партий товара – consignments  
  •	Таблица покупателей – customers  
  •	Таблица поставщиков – purveyors  
  •	Таблица продаж – sales  
  •	Таблица техники – technique  
 
### 2. Добавим необходимые связи между таблицами c каскадным изменением зависимых строк в подчиненной таблице при изменении строк в родительской
•	связь 1:М между подчиненной таблицей accounts и родительской таблицей customers  
```sql
CONSTRAINT [customer_fk] FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customers] ([customer_id]) ON UPDATE CASCADE
```
•	связь 1:М между подчиненной таблицей sales и родительской таблицей accounts
```sql
CONSTRAINT [account_fk] FOREIGN KEY ([account_id]) REFERENCES [dbo].[accounts] ([account_id]) ON UPDATE CASCADE
```
•	связь 1:М между подчиненной таблицей sales и родительской таблицей technique 
```sql
CONSTRAINT [tech_fk] FOREIGN KEY ([tech_id]) REFERENCES [dbo].[technique] ([tech_id]) ON UPDATE CASCADE
```
•	связь 1:М между подчиненной таблицей technique и родительской таблицей category_names
```sql
CONSTRAINT [category_fk] FOREIGN KEY ([category_id]) REFERENCES [dbo].[category_names] ([category_id]) ON UPDATE CASCADE
```
•	связь 1:М между подчиненной таблицей technique и родительской таблицей consignments
```sql
CONSTRAINT [consignment_fk] FOREIGN KEY ([consignment_id]) REFERENCES [dbo].[consignments] ([consignment_id]) ON UPDATE CASCADE
```
•	связь 1:М между подчиненной таблицей consignments и родительской таблицей purveyors
```sql
CONSTRAINT [purveyor_fk] FOREIGN KEY ([purveyor_id]) REFERENCES [dbo].[purveyors] ([purveyor_id]) ON UPDATE CASCADE
```
![image](https://github.com/user-attachments/assets/23da64a8-5a2c-4493-a1b3-d6b79909d39a)

### 3. Добавим проверочные ограничения 
•	Таблица счетов – accounts 
Сумма не меньше 0. Скидка между 0 и 100.
```sql
 constraint sum_ck check (sum >= 0),
	constraint disc_ck check (disc >= 0 and disc <= 100)
```
•	Таблица названий категорий техники – category names
Имя категории уникально.
```sql
 	UNIQUE NONCLUSTERED ([category_name] ASC)
```
•	Таблица покупателей – customers
Номер телефона и номер кредитного счета уникальны
```sql
 UNIQUE NONCLUSTERED ([phone_number] ASC),
    	UNIQUE NONCLUSTERED ([credit_account_id] ASC)
```
•	Таблица поставщиков – purveyors
Имя поставщика уникально
```sql
 UNIQUE NONCLUSTERED ([purveyor_name] ASC)
```
•	Таблица продаж – sales
Количество и сумма не меньше 0. Скидка между 0 и 100
```sql
 	constraint [cnt_chk] check (count >=0),
	constraint [total_chk] check (total >=0),
	constraint [d_chk] check (disc >=0 and disc <= 100)
```
•	Таблица техники – technique
Количество на складе, цена и период гарантии не меньше 0
```sql
 	constraint cnt_ck check (count_stock >= 0),
	constraint price_ck check (price >= 0),
	constraint per_ck check (guarantee_period >= 0)
```
### 4. Добавим последовательности для генераций уникальных ключей в таблицах
```sql
CREATE SEQUENCE [dbo].[account_id_seq]
    AS BIGINT
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
CREATE SEQUENCE [dbo].[categ_id_seq]
    AS BIGINT
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
CREATE SEQUENCE [dbo].[consig_id_seq]
    AS BIGINT
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
CREATE SEQUENCE [dbo].[customer_id_seq]
    AS BIGINT
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
CREATE SEQUENCE [dbo].[purveyor_id_seq]
    AS BIGINT
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
CREATE SEQUENCE [dbo].[tech_id_seq]
    AS BIGINT
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
```
### 5. Добавим процедуры для добавления, изменения данных в таблицах
При добавлении данных в таблицу генерируется новое значение для первичного ключа через последовательность. Пример
```sql
insert into dbo.category_names(	
	category_id,
	. . .
values(
	next value for dbo.categ_id_seq,
	. . .
```
•	Таблица счетов – accounts
```sql
 CREATE   procedure [dbo].add_account (
@a_i bigint,
@c_i  bigint,
@date nvarchar(40),
@disc  bigint,
@sum money)
as 
begin
	insert into dbo.accounts(	
	account_id,	
	customer_id,	
	date,	
	disc,	
	sum)
 values(
	@a_i,
	@c_i  ,
	convert(datetime, @date, 104),
	@disc  ,
	cast(@sum as money)
	)
end;

CREATE procedure del_acc(@a_i bigint)
as begin	
	delete from sales where account_id = @a_i;
	delete from accounts where account_id = @a_i;

	select 1 as r;
end
```
•	Таблица названий категорий техники – category names
```sql
 create   procedure [dbo].add_category (
@name nvarchar(40)
)
as 
begin
	insert into dbo.category_names(	
	category_id,
	category_name)
 values(
	next value for dbo.categ_id_seq,
	@name)
end;

CREATE   procedure upd_categ(@c_i nvarchar(20), @cat nvarchar(40))
as begin	
	update category_names set category_name = @cat where category_id = convert(bigint, @c_i);
	select 1 as r;
end
```
•	Таблица партий товара – consignments
```sql
 create   procedure [dbo].add_consig (
@purv bigint,
@date nvarchar(40)
)
as 
begin
	insert into dbo.consignments(	
	consignment_id,
	purveyor_id,
	date)
 values(
	next value for dbo.consig_id_seq,
	@purv,
	convert(datetime, @date, 104))
end;

CREATE   procedure upd_cons(
	@con_id nvarchar(50),
	@purv nvarchar(50),
	@dat nvarchar(50)
) as
begin
	if (convert(bigint, @con_id) in (select consignment_id from consignments)) begin
	update dbo.consignments set purveyor_id = convert(bigint, @purv) where consignment_id = convert(bigint, @con_id);
	update dbo.consignments set date = convert(datetime, @dat, 104) where consignment_id = convert(bigint, @con_id);
	end else insert into dbo.consignments (consignment_id, purveyor_id, date) values (convert(bigint, @con_id), convert(bigint, @purv),convert(datetime, @dat, 104));
end
```
•	Таблица покупателей – customers
```sql
 create   procedure [dbo].add_customer (
@f_name  nvarchar(30),
@s_name  nvarchar(30),
@t_name  nvarchar(30),
@p_s  bigint,
@p_i  bigint,
@phone  bigint,
@c_a  nvarchar(50))
as 
begin
	insert into dbo.customers (	
	customer_id,	
	second_name,	
	first_name,	
	third_name,	
	passport_series,	
	passport_id,	
	phone_number,	
	credit_account_id)
 values(
	next value for dbo.customer_id_seq,
	@s_name,
	@f_name,
	@t_name,
	@p_s,
	@p_i,
	@phone,
	@c_a
	)
end;

CREATE   procedure upd_cust(
	@cust_id nvarchar(50),
	@s_n nvarchar(30),
	@f_n nvarchar(30),
	@t_n nvarchar(30),
	@p_s nvarchar(50),
	@p_id nvarchar(50),
	@phone nvarchar(50),
	@cred nvarchar(50)
) as
begin
	if (convert(bigint, @cust_id) in (select customer_id from customers)) 
	begin
		update dbo.customers set second_name = @s_n where customer_id = convert(bigint, @cust_id);
		update dbo.customers set first_name = @f_n where customer_id = convert(bigint, @cust_id);
		update dbo.customers set third_name = @t_n where customer_id = convert(bigint, @cust_id);
		update dbo.customers set passport_series = convert(bigint, @p_s) where customer_id = convert(bigint, @cust_id);
		update dbo.customers set passport_id = convert(bigint, @p_id) where customer_id = convert(bigint, @cust_id);
		update dbo.customers set phone_number = convert(bigint, @phone) where customer_id = convert(bigint, @cust_id);
		update dbo.customers set credit_account_id = @cred where customer_id = convert(bigint, @cust_id);
	end else 
		insert into dbo.customers (customer_id, second_name, first_name, third_name, passport_series, passport_id, phone_number, credit_account_id)
		values (@cust_id, @s_n, @f_n, @t_n, convert(bigint, @p_s), convert(bigint, @p_id), convert(bigint, @phone), @cred)
end
```
•	Таблица поставщиков – purveyors
```sql
 create   procedure [dbo].add_purveyor (
@name nvarchar(40)
)
as 
begin
	insert into dbo.purveyors(	
	purveyor_id,
	purveyor_name)
 values(
	next value for dbo.purveyor_id_seq,
	@name)
end;

CREATE   procedure upd_purv(@p_i nvarchar(20), @p nvarchar(40))
as begin	
	update purveyors set purveyor_name = @p where purveyor_id = convert(bigint, @p_i);
	select 1 as r;
end
```
•	Таблица продаж – sales
```sql
 create   procedure add_sale (
@a_i bigint,
@t_i bigint,
@cnt bigint,
@disc bigint,
@total float
) as
declare
	@b bigint,
	 @st_cnt bigint;	 
begin 
	select @st_cnt = count_stock - @cnt from technique where tech_id = @t_i;

	if @st_cnt < 0  
	begin
		select @b = 0;
	end 
	else 
	begin
		select @b = 1;
		update technique set count_stock = @st_cnt where tech_id = @t_i;
		insert into sales (
			account_id,
			tech_id,
			count,
			disc,
			total
			) values(
			@a_i,
			@t_i,
			@cnt,
			@disc,
			@total);
	end
	select @b as r; 
end
```
•	Таблица техники – technique
```sql
  create   procedure [dbo].add_tech (
@model nvarchar(50),
@categ bigint,
@dat nvarchar(40),
@period bigint,
@price float,
@consig bigint,
@cnt bigint
)
as 
begin
	insert into dbo.technique(
	tech_id,
	model,
	category_id,
	issue_date,
	guarantee_period,
	price,
	consignment_id,
	count_stock)
 values(
	next value for dbo.tech_id_seq,
	@model,
	@categ,
	convert(datetime, @dat, 104),
	@period,
	@price,
	@consig,
	@cnt
	)
end;

create   procedure upd_tech(@t_i bigint, @cnt bigint)
as begin	
	update technique set count_stock = count_stock + @cnt where tech_id = @t_i;
	select 1 as r;
end
```
## 2. Вывод данных из SQL Server на форму C#
Вывод данных в каждой форме реализован одинаково, поэтому рассмотрим пример заполнения DataGridView в форме ввода данных названия техники Form6_2 из таблицы category_names
### 1. Инициализация 
Инициализируем переменные подключения к SQL Server, чтения потока данных из базы данных и поле data, где мы будем хранить данные из базы данных
```c#
        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        private List<string[]> data = null;
```
### 2. Извлекаем данные из базы данных
Подключаемся к нашей базе данных
```c#
connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
```
Открываем подключение
```c#
       connection.Open();
```
Добавляем запрос в переменную выполнения инструкции и выполняем ее в переменную чтения данных
```c#
       string query = "select * from category_names order by category_id";

       SqlCommand command = new SqlCommand(query, connection);

       reader = command.ExecuteReader();
```
Считываем каждую строку из reader и сохраняем все в data
```c#
       while (reader.Read())
            {
       	      data.Add(new string[2]);
              data[data.Count - 1][0] = reader[0].ToString().Trim();
              data[data.Count - 1][1] = reader[1].ToString().Trim();
            }
```
Закрываем подключение и чтение потока, выводим извлеченные данные
```c#
       reader.Close();
       connection.Close();
       printData(data);
```
### 3. Вывод данных (1 вариант: простой)
Каждую строку из data добавляем в dataGridView1
```c#
private void printData(List<string[]> d)
        {
            dataGridView1.Rows.Clear();
            foreach (string[] s in d)
                dataGridView1.Rows.Add(s);
        }
 ```
### 4. Вывод данных (2 вариант: в DataGridView ComboBox)
Когда нужно поместить данные в Combo Box в DataGridView, мы также инициализируем два списка (диапазона значений): что хотим отображать в Combo Box и значение, закрепленное за этим элементом
```c#
        private List<string> purv_id = null;
        private List<string> purv_name = null;
```
Такое же извлечение данных в эти списки
```c#
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
            string query = "select * from purveyors order by purveyor_id;";

            SqlCommand command = new SqlCommand(query, connection);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                purv_id.Add(reader[0].ToString().Trim());
                purv_name.Add(reader[1].ToString().Trim());
            }

            reader.Close();

            connection.Close();
```
В printData теперь добавляем вручную каждый столбец с указанием типа каждой ячейки. Далее каждую строку данных из data выводим в dataGridView, помещая диапазон значений в ячейку Combo Box и присваивая этой ячейке значение из этого диапазона, равное соответствующему значению из data
```c#
private void printData(List<string[]> d)
        {
            dataGridView1.Rows.Clear();

            //Добавление столбца

            DataGridViewColumn column1 = new DataGridViewColumn();
            column1.HeaderText = "Номер партии";
            column1.Name = "Column1";
            column1.CellTemplate = new DataGridViewTextBoxCell();
            column1.ReadOnly = true;
            dataGridView1.Columns.Add(column1);

            DataGridViewColumn column2 = new DataGridViewColumn();
            column2.HeaderText = "Поставщик";
            column2.Name = "Column2";
            column2.CellTemplate = new DataGridViewComboBoxCell();
            dataGridView1.Columns.Add(column2);

            DataGridViewColumn column3 = new DataGridViewColumn();
            column3.HeaderText = "Дата";
            column3.Name = "Column3";
            column3.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(column3);

            //Добавление данных

            foreach (string[] s in d)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell cell_A = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cell_B = new DataGridViewComboBoxCell();
                DataGridViewTextBoxCell cell_C = new DataGridViewTextBoxCell();

                cell_A.Value = s[0];
                row.Cells.Add(cell_A);

                cell_B.Items.AddRange(purv_name.ToArray());
                cell_B.Value = s[2];
                row.Cells.Add(cell_B);

                cell_C.Value = s[3];
                row.Cells.Add(cell_C);

                this.dataGridView1.Rows.Add(row);
            }
        }
```
## 3. Изменение данных в таблицах 
### 1. Изменение данных через хранимые процедуры
Рассмотрим на примере изменения таблицы category_names
Откроем подключение
```c#
connection.Open();
```
Для каждой строки в dataGridView. 
Если данные о категории уже были в таблице, то обновляем таблицу через процедуру upd_categ, изменяя данные по первичному ключу.
```c#
            foreach(DataGridViewRow r in dataGridView1.Rows)
            {
                if (r.Cells[0].Value != null)
                {
                    SqlCommand command = new SqlCommand("upd_categ", connection);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter ci = new SqlParameter
                    {
                        ParameterName = "@c_i",
                        Value = r.Cells[0].Value.ToString()
                    };
                    command.Parameters.Add(ci);

                    SqlParameter sn = new SqlParameter
                    {
                        ParameterName = "@cat",
                        Value = r.Cells[1].Value.ToString()
                    };
                    command.Parameters.Add(sn);

                    command.ExecuteNonQuery();
                }
```
Если же данных о записи не было изначально в таблице, то добавляем в таблицу новую запись через процедуру add_category
```c#
                else if (r.Cells[1].Value != null)
                {
                    SqlCommand command = new SqlCommand("add_category", connection);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter sn = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = r.Cells[1].Value.ToString()
                    };
                    command.Parameters.Add(sn);

                    command.ExecuteNonQuery();
                }
            }
```
Закрываем подключение
```c#
            connection.Close();
```
### 2. Изменение данных через инструкции
Рассмотрим на примере изменения таблицы technique
Создаем и открываем подключение к базе 
```c#
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            connection.Open();
```
Для каждой строки создаем команду на выполнение запроса
```c#
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"update technique set model = @mod,category_id = @cat,issue_date = convert(datetime, @dat, 104),guarantee_period = @per,price = @pr,consignment_id = @con,count_stock = @cnt,image = @img where tech_id = " + row.Cells[0].Value.ToString();

Добавим параметры в эту инструкцию, извлекая новые данные из dataGridView
                command.Parameters.Add("@mod", SqlDbType.NVarChar);
                command.Parameters["@mod"].Value = row.Cells[2].Value.ToString();

                int a = 0;
                command.Parameters.Add("@cat", SqlDbType.BigInt);
                for (int i = 0; i < categ.Count; i++)
                {
                    a = categ[i] == row.Cells[1].Value.ToString() ? Int32.Parse(categ_id[i]) : a;
                }
                if (a != 0)
                    command.Parameters["@cat"].Value = a;
                else command.Parameters["@cat"].Value = DBNull.Value;

                command.Parameters.Add("@dat", SqlDbType.DateTime);
                command.Parameters["@dat"].Value = row.Cells[3].Value.ToString();

                command.Parameters.Add("@per", SqlDbType.BigInt);
                command.Parameters["@per"].Value = Int32.Parse(row.Cells[4].Value.ToString());

                command.Parameters.Add("@pr", SqlDbType.BigInt);
                command.Parameters["@pr"].Value = Int32.Parse(row.Cells[5].Value.ToString());

                command.Parameters.Add("@con", SqlDbType.BigInt);
                a = 0;
                for (int i = 0; i < consig.Count; i++)
                {
                    a = consig[i] == row.Cells[6].Value.ToString() ? Int32.Parse(consig_id[i]) : a;
                }
                if (a != 0)
                    command.Parameters["@con"].Value = a;
                else command.Parameters["@con"].Value = DBNull.Value;

                command.Parameters.Add("@cnt", SqlDbType.BigInt);
                command.Parameters["@cnt"].Value = Int32.Parse(row.Cells[7].Value.ToString());

                command.Parameters.Add("@img", SqlDbType.Image);
                if (images[row.Index] == errPic)
                {
                    command.Parameters["@img"].Value = DBNull.Value;
                }
                else
                {
                    command.Parameters["@img"].Value = images[row.Index];
                }

                command.ExecuteNonQuery();
            }
```
Закрываем поключение
```c#
            connection.Close();
            label14.Text = "Изменения сохранены";
            udp_data();
```
