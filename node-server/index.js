const express = require("express");
const cors = require("cors");
const bodyParser = require("body-parser");
//
const axios = require("axios"); // to call your C# API
const path = require("path");
const mongoose = require("mongoose");
require("dotenv").config();

const app = express();
app.use(cors());
app.use(bodyParser.json());
app.use(express.json());
app.use(express.static(path.join(__dirname, 'public')));

//TOSET Views:
app.set("view engine", "ejs");
app.set("views", path.join(__dirname, "views"));


// MongoDB Connection
mongoose.connect(process.env.MONGO_URI, {
    useNewUrlParser: true,
    useUnifiedTopology: true
}).then(() => console.log("MongoDB Connected"))
    .catch(err => console.error(err));
//route defs
const authRoutes = require("./routes/api/auth");
const userapiRoutes = require("./routes/api/users");
const deviceapiRoutes = require("./routes/api/users");
const deviceRoutes = require("./routes/views/devices");
const usersRoute = require('./routes/views/users');

app.get("/", (req, res) => {
    res.send("Node.js API is running!");
});

app.use('/users', usersRoute);
app.use("/api/users", userapiRoutes);
app.use("/api/devices", deviceapiRoutes);
app.use("/devices", deviceRoutes);
app.use("/api/auth", authRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Node.js Server running on port ${PORT}`);
    console.log("Serving static files from:", path.join(__dirname, 'public'));
});
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).send(`Server error: ${err.message}`);
  });
  
// logging for all requests
app.use((req, res, next) => {
    console.log(`[REQ] ${req.method} ${req.url}`);
    next();
});

app.get('/debug-static', (req, res) => {
    res.sendFile(path.join(__dirname, 'public/styles/style.css'));
});
