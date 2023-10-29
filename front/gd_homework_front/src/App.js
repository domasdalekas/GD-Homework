import React, { useState, useEffect } from 'react';
import Category from './Category';
import SubcategoriesList from './Subcategories';
import {ReactComponent as Logo} from './images/logo.svg'

function App() {
  const [data, setData] = useState([]);
  const [selectedSubcategoryData, setSelectedSubcategoryData] = useState(null);
  const [svgWidth, setSvgWidth] = useState(0);
  const [svgHeight, setSvgHeight] = useState(0);
  const url = "https://localhost:44325/";

  useEffect(() => {
    fetch(url + "api/getCategoriesAndSubcategories")
      .then((response) => response.json())
      .then((data) => setData(data))
      .catch((error) => {
        console.error(error);
      });
  }, []);

  useEffect(() => {
    const handleResize = () => {
      const container = document.getElementById('svg-container');
      if (container) {
        const rect = container.getBoundingClientRect();
        setSvgWidth(rect.width);
        setSvgHeight(rect.height);
      }
    };

    handleResize();

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
    }, []);

  return (
    <>
    <div className="App">
      <div className="container">
          <div id="svg-container" className="svg-container">
            <svg>
              <Logo width={svgWidth} height={svgHeight}/>
            </svg>
          </div>
      </div>
      <div className="data-container">
        <div className="dropdown-container">
          {data.map((category, index) => (
            <Category key={index} {...category} selectedSubcategoryData={selectedSubcategoryData} setSelectedSubcategoryData={setSelectedSubcategoryData}/>
          ))}
        </div>
        <div className="table-container">
          {selectedSubcategoryData && (
            <SubcategoriesList selectedSubcategoryData={selectedSubcategoryData} />
          )}
        </div>
    </div>
    </div>
    </>
  );
}

export default App;
