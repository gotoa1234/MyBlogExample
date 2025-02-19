CREATE DATABASE IF NOT EXISTS Bank_B;
USE Bank_B;
CREATE TABLE accounts (
    id INT PRIMARY KEY AUTO_INCREMENT,
    account_number VARCHAR(20) NOT NULL UNIQUE,
    balance DECIMAL(18,2) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO accounts (account_number, balance) VALUES
('B_Louis', 500.00);