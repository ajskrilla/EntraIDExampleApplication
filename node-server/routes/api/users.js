const express = require("express");
const axios = require("axios");
const User = require("../../models/User");
const router = express.Router();

// Fetch users from C# backend & store in MongoDB

router.get("/", async (req, res) => {
    try {
        const response = await axios.get("https://localhost:7120/api/users");
        const users = response.data;

        /*
        await User.insertMany(users.map(user => ({
            displayName: user.displayName,
            email: user.mail,
            azureId: user.id
        })), { ordered: false }).catch(() => { });
        */
        res.json(users);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

module.exports = router;

