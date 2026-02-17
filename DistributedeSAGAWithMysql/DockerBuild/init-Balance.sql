CREATE DATABASE IF NOT EXISTS Balance;
USE Balance;

-- 帳戶餘額
CREATE TABLE AccountBalance (
    MemberId    BIGINT PRIMARY KEY,
    Balance     DECIMAL(18,2) NOT NULL
);

-- 扣款 / 退款流水
CREATE TABLE BalanceTransaction (
    TxId        BIGINT PRIMARY KEY AUTO_INCREMENT,
    MemberId    BIGINT NOT NULL,
    Amount      DECIMAL(18,2) NOT NULL, -- 正數=扣款，負數=退款
    SagaId      VARCHAR(50) NOT NULL,
	PurchaseId  BIGINT NOT NULL,
    CreatedAt  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO AccountBalance(MemberId, Balance)
VALUES(1, 200000);