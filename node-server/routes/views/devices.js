const express = require("express");
const axios = require("axios");
const https = require("https");
const router = express.Router();
const agent = new https.Agent({ rejectUnauthorized: false });

router.get("/", async (req, res) => {
    try {
        console.log("Fetching devices from C# backend...");
        const response = await axios.get("https://localhost:7120/api/devices/db", {
            httpsAgent: agent
        });

        const devices = response.data;
        console.log("Devices received:", devices.length);
        res.render("devices", { devices });

    } catch (error) {
        console.error("Error rendering devices page:", error.message);
        res.status(500).send("Error loading devices");
    }
});


module.exports = router;
