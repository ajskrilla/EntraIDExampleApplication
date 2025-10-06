const express = require("express");
const Credential = require("../../models/Credentials");
const router = express.Router();

// Save Azure Credentials
router.post("/", async (req, res) => {
    try {
        console.log("/api/auth route hit");

        const { clientId, tenantId, clientSecret } = req.body;

        if (!clientId || !tenantId || !clientSecret) {
            return res.status(400).json({ error: "Missing required fields" });
        }

        // Store credentials in DB
        await Credential.deleteMany({}); // Clear existing credentials (optional)
        /*
        const credential = new Credential({ clientId, tenantId, clientSecret });
        await credential.save();
        */
        await Credential.create({ tenantId, clientId, clientSecret })
            .then(() => console.log("Credential saved to DB"))
            .catch(err => console.error("DB Save Error:", err));
        res.json({ message: "Azure credentials stored successfully" });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Retrieve Azure Credentials
router.get("/", async (req, res) => {
    try {
        const credential = await Credential.findOne();
        if (!credential) {
            return res.status(404).json({ error: "No credentials found" });
        }
        res.json(credential);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

module.exports = router;
