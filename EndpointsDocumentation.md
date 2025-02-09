# Binance Dashboard API

Below is a quick reference index. Click on any endpoint to jump to its documentation:

- [1. GetBalance](#1-getbalance)
- [2. GetAccountTrades](#2-getaccounttrades)
- [3. GetIncomeHistory](#3-getincomehistory)
- [4. GetOpenPositions](#4-getopenpositions)
- [5. GetOpenOrders](#5-getopenorders)
- [6. GetPositionHistory](#6-getpositionhistory)
- [7. GetBalanceSnapshot](#7-getbalancesnapshot)
- [8. GetDailyPNL](#8-getdailypnl)
- [9. GetMonthlySummary](#9-getmonthlysummary)
- [10. GetHistory](#10-gethistory)
- [11. GetLastUpdatedTime](#11-getlastupdatedtime)

---

---

## 1. GetBalance

### Description

The `GetBalance` endpoint provides the current balance of the Binance futures account along with the last update timestamp. This allows users to track their account balance without the risk of executing trades.

### HTTP Request

**`GET /Account/GetBalance`**

### Response

**Response Type:** `FuturesAccountBalanceResponseDto`

| Property     | Type       | Description                                           |
| ------------ | ---------- | ----------------------------------------------------- |
| `balance`    | `float`    | The current balance available in the futures account. |
| `updateTime` | `DateTime` | The timestamp of the last balance update.             |

### Example Response

```json
{
  "balance": 10543.75,
  "updateTime": "2025-01-25T14:30:00Z"
}
```

### Usage

This endpoint is useful for tracking the overall available balance in the futures account and verifying the most recent balance update time.

## 2. GetAccountTrades

### Description

The `GetAccountTrades` endpoint retrieves a list of past trades executed on the Binance futures account. This information helps users analyze their trading history and performance without placing new trades.

### HTTP Request

**`GET /Account/GetAccountTrades`**

### Response

**Response Type:** `FuturesAccountTradeResponseDto[]`

| Property        | Type       | Description                                 |
| --------------- | ---------- | ------------------------------------------- |
| `symbol`        | `string`   | The trading pair symbol (e.g., BTCUSDT).    |
| `id`            | `long`     | The unique trade identifier.                |
| `orderId`       | `long`     | The associated order ID for the trade.      |
| `side`          | `string`   | The trade side (BUY or SELL).               |
| `price`         | `float`    | The execution price of the trade.           |
| `quantity`      | `float`    | The amount of the asset traded.             |
| `realizedPnl`   | `float`    | The realized profit or loss from the trade. |
| `quoteQuantity` | `float`    | The value of the trade in quote asset.      |
| `commission`    | `float`    | The fee paid for the trade.                 |
| `positionSide`  | `string`   | The position side (LONG or SHORT).          |
| `buyer`         | `bool`     | Indicates if the trade was a buy.           |
| `maker`         | `bool`     | Indicates if the trade was a maker order.   |
| `time`          | `DateTime` | The timestamp of the trade execution.       |

### Example Response

```json
[
  {
    "symbol": "BTCUSDT",
    "id": 12345678,
    "orderId": 98765432,
    "side": "BUY",
    "price": 45000.5,
    "quantity": 0.1,
    "realizedPnl": 50.25,
    "quoteQuantity": 4500.05,
    "commission": 2.5,
    "positionSide": "LONG",
    "buyer": true,
    "maker": false,
    "time": "2025-01-25T15:00:00Z"
  }
]
```

### Usage

This endpoint is useful for reviewing historical trades and analyzing performance trends based on executed orders.

## 3. GetIncomeHistory

### Description

The `GetIncomeHistory` endpoint provides a detailed record of income-related transactions in the Binance futures account. This includes earnings from trades, commissions, funding fees, and other sources of income.

### HTTP Request

**`GET /Account/GetIncomeHistory`**

### Response

**Response Type:** `FuturesIncomeHistoryResponseDto[]`

| Property     | Type       | Description                                         |
| ------------ | ---------- | --------------------------------------------------- |
| `symbol`     | `string`   | The trading pair symbol (e.g., BTCUSDT).            |
| `incomeType` | `string`   | The type of income (e.g., COMMISSION, FUNDING_FEE,REALIZED_PNL). |
| `income`     | `float`    | The amount of income received.                      |
| `asset`      | `string`   | The asset in which the income was received.         |
| `info`       | `string`   | Additional details about the income source.         |
| `time`       | `DateTime` | The timestamp when the income was recorded.         |
| `tranId`     | `long`     | The transaction ID associated with the income.      |
| `tradeId`    | `long`     | The trade ID related to the income, if applicable.  |

### Example Response

```json
[
  {
    "symbol": "BTCUSDT",
    "incomeType": "REALIZED_PNL",
    "income": 12.5,
    "asset": "USDT",
    "info": "13579604",
    "time": "2025-01-25T18:45:00Z",
    "tranId": 987654321,
    "tradeId": 123456789
  }
]
```

### Usage

This endpoint helps users track various sources of income from their Binance futures trading activity, such as funding fees, trading commissions, and other earnings.

## 4. GetOpenPositions

### Description

The `GetOpenPositions` endpoint provides details about currently open positions in the Binance futures account. This information helps users track their active positions, unrealized profits, and liquidation risks.

### HTTP Request

**`GET /Account/GetOpenPositions`**

### Response

**Response Type:** `FutureOpenPositionsResponseDto[]`

| Property           | Type     | Description                                            |
| ------------------ | -------- | ------------------------------------------------------ |
| `symbol`           | `string` | The trading pair symbol (e.g., BTCUSDT).               |
| `positionSide`     | `string` | The position side (LONG or SHORT).                     |
| `entryPrice`       | `float`  | The price at which the position was opened.            |
| `unRealizedProfit` | `float`  | The unrealized profit or loss of the position.         |
| `liquidationPrice` | `float`  | The price at which the position will be liquidated.    |
| `notional`         | `float`  | The value of the position in terms of the quote asset. |

### Example Response

```json
[
  {
    "symbol": "BTCUSDT",
    "positionSide": "LONG",
    "entryPrice": 45000.0,
    "unRealizedProfit": 250.75,
    "liquidationPrice": 30000.0,
    "notional": 9000.0
  }
]
```

### Usage

This endpoint is useful for monitoring currently open positions, assessing potential liquidation risks, and tracking unrealized profits or losses.

## 5. GetOpenOrders

### Description

The `GetOpenOrders` endpoint retrieves details of currently open orders in the Binance futures account. This allows users to monitor pending buy or sell orders that have not yet been executed.

### HTTP Request

**`GET /Account/GetOpenOrders`**

### Response

**Response Type:** `FuturesOpenOrdersResponseDto[]`

| Property    | Type       | Description                                        |
| ----------- | ---------- | -------------------------------------------------- |
| `price`     | `float`    | The price at which the order was placed.           |
| `symbol`    | `string`   | The trading pair symbol (e.g., BTCUSDT).           |
| `time`      | `DateTime` | The timestamp when the order was created.          |
| `entryType` | `string`   | The entry type of the order (e.g., LIMIT, MARKET). |
| `orderType` | `string`   | The type of order placed (e.g., BUY, SELL).        |
| `amount`    | `float`    | The amount of the asset involved in the order.     |

### Example Response

```json
[
  {
    "price": 42000.5,
    "symbol": "BTCUSDT",
    "time": "2025-01-26T12:00:00Z",
    "entryType": "LIMIT",
    "orderType": "BUY",
    "amount": 0.5
  }
]
```

### Usage

This endpoint helps users track their open orders and make informed decisions about modifying or canceling them based on market conditions.

## 6. GetPositionHistory

### Description

The `GetPositionHistory` endpoint provides a historical record of closed positions in the Binance futures account. It allows users to review past trades, including entry and exit prices, realized profit and loss, and timestamps.

### HTTP Request

**`GET /Account/GetPositionHistory`**

### Response

**Response Type:** `PositionHistoryResponseDto[]`

| Property        | Type       | Description                                         |
| --------------- | ---------- | --------------------------------------------------- |
| `symbol`        | `string`   | The trading pair symbol (e.g., BTCUSDT).            |
| `positionSide`  | `string`   | The position side (LONG or SHORT).                  |
| `entryPrice`    | `float`    | The price at which the position was opened.         |
| `avgClosePrice` | `float`    | The average price at which the position was closed. |
| `openTime`      | `DateTime` | The timestamp when the position was opened.         |
| `closeTime`     | `DateTime` | The timestamp when the position was closed.         |
| `pnl`           | `float`    | The realized profit or loss from the position.      |

### Example Response

```json
[
  {
    "symbol": "BTCUSDT",
    "positionSide": "LONG",
    "entryPrice": 45000.0,
    "avgClosePrice": 46000.0,
    "openTime": "2025-01-20T10:00:00Z",
    "closeTime": "2025-01-22T15:30:00Z",
    "pnl": 1000.5
  }
]
```

### Usage

This endpoint is useful for analyzing historical positions, tracking performance, and identifying trading patterns.

## 7. GetBalanceSnapshot

### Description

The `GetBalanceSnapshot` endpoint provides a historical snapshot of the Binance futures account balance for specific dates. This allows users to track balance changes over time for better financial planning and analysis.

### HTTP Request

**`GET /Account/GetBalanceSnapshot`**

### Response

**Response Type:** `BalanceSnapshotResponseDto[]`

| Property  | Type       | Description                                     |
| --------- | ---------- | ----------------------------------------------- |
| `date`    | `DateOnly` | The date of the balance snapshot.               |
| `balance` | `float`    | The account balance recorded on the given date. |

### Example Response

```json
[
  {
    "date": "2025-01-25",
    "balance": 10450.75
  },
  {
    "date": "2025-01-26",
    "balance": 10700.25
  }
]
```

### Usage

This endpoint is useful for monitoring account balance trends, generating reports, and assessing financial growth over time.

## 8. GetDailyPNL

### Description

The `GetDailyPNL` endpoint provides daily profit and loss (PNL) data for the Binance futures account. This allows users to track their daily trading performance and assess profitability over time.

### HTTP Request

**`GET /Account/GetDailyPNL`**

### Response

**Response Type:** `DailyPNLResponseDTO[]`

| Property      | Type       | Description                                       |
| ------------- | ---------- | ------------------------------------------------- |
| `date`        | `DateOnly` | The date for which the PNL is recorded.           |
| `pnl`         | `float`    | The profit or loss for the specified date.        |
| `lastUpdated` | `DateTime` | The timestamp when the PNL data was last updated. |

### Example Response

```json
[
  {
    "date": "2025-01-25",
    "pnl": 150.75,
    "lastUpdated": "2025-01-26T02:00:00Z"
  },
  {
    "date": "2025-01-26",
    "pnl": -50.25,
    "lastUpdated": "2025-01-26T22:30:00Z"
  }
]
```

### Usage

This endpoint is useful for tracking daily trading results, analyzing performance trends, and making informed decisions based on historical PNL data.

## 9. GetMonthlySummary

### Description

The `GetMonthlySummary` endpoint provides a summary of trading performance for each month in the Binance futures account. This includes the total profit and loss (PNL) and the daily average PNL for the given month.

### HTTP Request

**`GET /Account/GetMonthlySummary`**

### Response

**Response Type:** `MonthlySummaryResponseDto[]`

| Property       | Type    | Description                                         |
| -------------- | ------- | --------------------------------------------------- |
| `month`        | `Int16` | The month for which the summary is provided (1-12). |
| `year`         | `Int16` | The year corresponding to the summary.              |
| `pnl`          | `float` | The total profit or loss for the month.             |
| `dailyAverage` | `float` | The average daily profit or loss for the month.     |

### Example Response

```json
[
  {
    "month": 1,
    "year": 2025,
    "pnl": 4520.75,
    "dailyAverage": 145.83
  },
  {
    "month": 2,
    "year": 2025,
    "pnl": 3120.5,
    "dailyAverage": 111.43
  }
]
```

### Usage

This endpoint helps users evaluate their monthly trading performance, analyze trends over time, and set goals based on their financial progress.

## 10. GetHistory

### Description

The `GetHistory` endpoint provides a comprehensive summary of trading activity for specific dates in the Binance futures account. It includes details such as commissions, profit and loss (PNL), income, and multipliers.

### HTTP Request

**`GET /Account/GetHistory`**

### Response

**Response Type:** `HistoryResponseDto[]`

| Property | Type             | Description                                         |
| -------- | ---------------- | --------------------------------------------------- |
| `date`   | `DateOnly`       | The date for which the historical data is provided. |
| `data`   | `HistorySummary` | The summary of trading history for the date.        |

#### HistorySummary Object

| Property     | Type    | Description                                                                                                                          |
| ------------ | ------- | ------------------------------------------------------------------------------------------------------------------------------------ |
| `commission` | `float` | The total commission paid for the trades.                                                                                            |
| `pnl`        | `float` | The profit or loss for the day.                                                                                                      |
| `income`     | `float` | The total income earned from trading.                                                                                                |
| `multiplier` | `float` | Multiples the income by a fixed value, can be configured. Used to simulate different capital allocations for hypothetical scenarios. |

### Example Response

```json
[
  {
    "date": "2025-01-25",
    "data": {
      "commission": 50.75,
      "pnl": 1250.5,
      "income": 300.0,
      "multiplier": 3000.0
    }
  },
  {
    "date": "2025-01-26",
    "data": {
      "commission": 45.25,
      "pnl": 950.0,
      "income": 280.0,
      "multiplier": 2800.0
    }
  }
]
```

### Usage

This endpoint helps users track their daily trading activity, including commissions paid, realized profits and losses, and income earned.

## 11. GetLastUpdatedTime

### Description

The `GetLastUpdatedTime` endpoint returns the most recent update timestamp for the Binance futures account data. A background service (running every 5 minutes by default, but configurable in the application settings) fetches data from the Binance endpoints to avoid rate-limiting and ensure fast updates. This means there may be a slight delay in the displayed data compared to real-time.

### HTTP Request

**`GET /Account/GetLastUpdatedTime`**

### Response

| Property          | Type       | Description                            |
| ----------------- | ---------- | -------------------------------------- |
| `lastUpdatedTime` | `DateTime` | The timestamp of the last data update. |

### Example Response

```json
{
  "lastUpdatedTime": "2025-01-26T15:45:00Z"
}
```

### Usage

This endpoint is useful for determining when the account data was last refreshed. Due to the scheduled background service, the data is updated periodically rather than in real time, helping to avoid rate limits while providing timely account information.
