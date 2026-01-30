CREATE DATABASE IF NOT EXISTS Member;
USE Member;

CREATE TABLE Member (
    MemberId        BIGINT PRIMARY KEY,
    Name            VARCHAR(50) NOT NULL,
    TotalSpent      DECIMAL(18,2) NOT NULL DEFAULT 0
);

CREATE TABLE Product (
    ProductId       BIGINT PRIMARY KEY,
    ProductName     VARCHAR(100) NOT NULL,
    Price           DECIMAL(18,2) NOT NULL,
	StockCount           INT DEFAULT 0 NOT NULL
);

CREATE TABLE Purchase (
    PurchaseId      BIGINT PRIMARY KEY AUTO_INCREMENT,
    SagaId          VARCHAR(50) NOT NULL,
    MemberId        BIGINT NOT NULL,
    ProductId       BIGINT NOT NULL,
    Amount          DECIMAL(18,2) NOT NULL,
    Status          VARCHAR(20) NOT NULL, -- SUCCESS / FAILED
    CreatedAt       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY UK_Saga_Purchase (SagaId)
);

INSERT INTO Member(MemberId, Member.NAME, TotalSpent)
Values(1, 'MilkTeaGreen', 0);

INSERT INTO Product(ProductId, ProductName, Price, StockCount)
VALUES(1001, '電視', 38000, 10);