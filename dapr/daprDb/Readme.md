

CREATE TABLE products (id SERIAL PRIMARY KEY, name VARCHAR (200) UNIQUE NOT NULL, availability date, price float, currency VARCHAR(10), details VARCHAR(1000));
