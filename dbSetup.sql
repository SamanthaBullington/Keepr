CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) default charset utf8 COMMENT '';
CREATE TABLE IF NOT EXISTS keeps(
  id INT AUTO_INCREMENT NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) NOT NULL COMMENT 'keep Name',
  creatorId VARCHAR(255) NOT NULL COMMENT 'Keep creatorId',
  img varchar(255) COMMENT 'Keep Image Url',
  description VARCHAR(255) COMMENT 'Keep Description',
  views int NOT NULL DEFAULT 0 COMMENT  'Keep views',
  shares int NOT NULL DEFAULT 0 COMMENT  'Keep shares',
  keeps int NOT NULL DEFAULT 0 COMMENT  'Times keeps added to vaults',
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) default charset utf8 COMMENT '';
CREATE TABLE IF NOT EXISTS vaults(
  id INT AUTO_INCREMENT NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) NOT NULL COMMENT 'Vault Name',
  description VARCHAR(255) COMMENT 'Vault Description',
  isPrivate TINYINT COMMENT 'Bool value for private',
  creatorId VARCHAR(255) NOT NULL COMMENT 'Vault CreatorId',
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) default charset utf8 COMMENT '';
CREATE TABLE vaultkeeps(
  id int NOT NULL primary key AUTO_INCREMENT comment 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  vaultId int NOT NULL comment 'vault Id',
  keepId int NOT NULL comment 'keep id',
  creatorId VARCHAR(255) NOT NULL COMMENT 'Vault CreatorId',
  FOREIGN KEY (vaultId) REFERENCES vaults(id) ON DELETE CASCADE,
  FOREIGN KEY (keepId) REFERENCES keeps(id) ON DELETE CASCADE,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) default charset utf8 comment '';