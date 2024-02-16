

CREATE TABLE products (id SERIAL PRIMARY KEY, name VARCHAR (200) UNIQUE NOT NULL, availability date, price float, currency VARCHAR(10), details VARCHAR(1000));

Response:
[[1,"Spoon","2024-02-17T00:00:00Z",330,"HUF","Warm"],[2,"Coat","2024-02-18T00:00:00Z",102,"HUF","Hot"],[3,"Table","2024-02-19T00:00:00Z",414,"HUF","Hot["]]

    \[[0-9]+,"[^"]*","[^"]*",[0-9]+,"[^"]*","[^"]*"\]