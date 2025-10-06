const express = require("express");
const axios = require("axios");
const https = require("https");
const router = express.Router();

const agent = new https.Agent({ rejectUnauthorized: false });

router.get("/", async (req, res) => {
    try {
        const response = await axios.get("https://localhost:7120/api/users/db", {
            httpsAgent: agent
        });

        const users = response.data;

        const page = parseInt(req.query.page) || 1;
        const pageSize = 50;
        const totalUsers = users.length;
        const totalPages = Math.ceil(totalUsers / pageSize);

        const paginatedUsers = users.slice((page - 1) * pageSize, page * pageSize);

        res.render("users", {
            users: paginatedUsers,
            currentPage: page,
            totalPages: totalPages
        });
    } catch (error) {
        console.error("Error rendering users page:", error.message);
        res.status(500).send("Error loading users");
    }
});


module.exports = router;
