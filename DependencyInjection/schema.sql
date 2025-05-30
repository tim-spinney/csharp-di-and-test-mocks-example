create table products
(
    id          integer primary key,
    description text,
    price       integer,
    quantity    integer
);

create table users
(
    id               integer primary key,
    name             text,
    shipping_address text
);

create table wallets
(
    id      integer primary key,
    user_id int not null,
    balance int default 0
);

