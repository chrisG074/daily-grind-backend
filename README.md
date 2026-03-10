# daily-grind-backend

A Node.js/Express backend service for the Daily Grind application. It provides a REST API and connects to a PostgreSQL database.

## Tech Stack

- **Runtime**: Node.js 18
- **Framework**: Express
- **Database**: PostgreSQL (via `pg`)
- **Container**: Docker

## Prerequisites

- [Node.js](https://nodejs.org/) v18 or later
- [npm](https://www.npmjs.com/)
- A running PostgreSQL instance (optional — the server starts without one)

## Installation

```bash
npm install
```

## Environment Variables

| Variable       | Description                                  | Required |
| -------------- | -------------------------------------------- | -------- |
| `DATABASE_URL` | PostgreSQL connection string                 | No       |
| `PORT`         | Port the server listens on (default: `5000`) | No       |

## Running Locally

```bash
# Start the server
npm start
```

The server will start on `http://localhost:5000` (or the port specified in `PORT`).

## Running with Docker

```bash
# Build the image
docker build -t daily-grind-backend .

# Run the container
docker run -p 5000:5000 -e DATABASE_URL=<your-db-url> daily-grind-backend
```

## API Endpoints

| Method | Path         | Description                                              |
| ------ | ------------ | -------------------------------------------------------- |
| `GET`  | `/health`    | Returns server and database connectivity status          |
| `GET`  | `/api/ping`  | Simple liveness check — returns `{ message: "Pong from backend" }` |

### `GET /health`

Returns the health status of the server and its database connection.

**Response (no DB configured)**
```json
{ "status": "ok", "db": "not-configured" }
```

**Response (DB connected)**
```json
{ "status": "ok", "db": "connected" }
```

**Response (DB error) — HTTP 500**
```json
{ "status": "error", "db": "disconnected", "message": "<error details>" }
```

### `GET /api/ping`

```json
{ "message": "Pong from backend" }
```
