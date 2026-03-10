const express = require("express");
const { Pool } = require("pg");

const app = express();
const port = Number(process.env.PORT) || 5000;

const databaseUrl = process.env.DATABASE_URL;
const pool = databaseUrl
  ? new Pool({ connectionString: databaseUrl })
  : null;

app.get("/health", async (_req, res) => {
  if (!pool) {
    return res.json({ status: "ok", db: "not-configured" });
  }

  try {
    await pool.query("SELECT 1");
    return res.json({ status: "ok", db: "connected" });
  } catch (error) {
    return res.status(500).json({ status: "error", db: "disconnected", message: error.message });
  }
});

app.get("/api/ping", (_req, res) => {
  res.json({ message: "Pong from backend" });
});

app.listen(port, () => {
  console.log(`Backend running on port ${port}`);
});
