import { useState, useEffect } from "react";
import { Route, Routes } from "react-router-dom";

import SideMenu from "./components/SideMenu/index";
import ClassDocDisplay from "./components/ClassDocDisplay/index";
import FunctionDocDisplay from "./components/FunctionDocDisplay/index";
import Overview from "./components/Overview";
import CodeCity from "./components/CodeCity";

import { RepoDescription } from "./components/types/description";

const App = () => {
  const [documentation, setDocumentation] = useState<RepoDescription | null>(
    null
  );

  useEffect(() => {
    const fetchDocumentation = async () => {
      try {
        const response = await fetch("./output.json");
        const doc = await response.json();
        setDocumentation(doc);
      } catch {
        try {
          const response = await fetch("./examples/example_output.json");
          const doc = await response.json();
          setDocumentation(doc);
        } catch (error) {
          console.log("Failed to fetch JSON documentation:", error);
        }
      }
    };
    fetchDocumentation();
  }, []);

  if (!documentation) {
    return null;
  }

  return (
    <div style={{ display: "flex", height: "100vh", flexDirection: "column" }}>
      <div style={{ display: "flex", flex: 1 }}>
        <SideMenu documentation={documentation} />
        <div style={{ flex: 1, padding: "20px" }}>
          <Routes>
            <Route
              path="/"
              element={<Overview overview={documentation.overview} />}
            />
            <Route
              path="/class/:className"
              element={<ClassDocDisplay documentation={documentation} />}
            />
            <Route
              path="/function/:functionName"
              element={<FunctionDocDisplay documentation={documentation} />}
            />
            <Route path="/codecity" element={<CodeCity />} />
          </Routes>
        </div>
      </div>
    </div>
  );
};

export default App;
