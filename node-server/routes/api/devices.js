const express = require("express");
const axios = require("axios");
const https = require("https");
const Device = require("../../models/Device");

const router = express.Router();

const agent = new https.Agent({ rejectUnauthorized: false });

router.get("/", async (req, res) => {
    try {
        //db endpoint to look at DB
        const response = await axios.get("https://localhost:7120/api/devices", {
            httpsAgent: agent
        });

        const devices = response.data;

        /*
        await Device.insertMany(devices.map(device => ({
            displayName: device.displayName,
            deviceId: device.id,
            operatingSystem: device.operatingSystem
        })), { ordered: false }).catch(() => { });
        */
        res.json(devices);
    } catch (error) {
        console.error("Device fetch error:", error.message);
        res.status(500).json({ error: error.message });
    }
});

module.exports = router;
