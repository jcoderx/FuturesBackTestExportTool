using FuturesBackTestExportTool.Model;
using FuturesBackTestExportTool.ReportModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace FuturesBackTestExportTool
{
    public class ExcelExport
    {
        private XSSFCellStyle commonStyle;
        private XSSFCellStyle titleStyle;
        private XSSFCellStyle headerStyle;
        private XSSFCellStyle warningStyle;

        private int leftRowIndex = 1;
        private int rightRowIndex = 1;

        //TODO 边框、单元格宽度、高度

        public void exportExcel(VarietyReport varietyReport)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("Sheet1");
            setColumnWidth(sheet);
            initStyle(workbook);

            //第一行，品种、起止时间
            XSSFRow row0 = (XSSFRow)sheet.CreateRow(0);
            setRowHeight(row0);
            XSSFCell cell = (XSSFCell)row0.CreateCell(0);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(varietyReport.varietyName + Variety.getAgreementName(varietyReport.agreement));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));
            cell = (XSSFCell)row0.CreateCell(1);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Utils.formatDate(varietyReport.startingDate) + "-" + Utils.formatDate(varietyReport.endingDate));

            List<CycleReport> cycleReports = varietyReport.cycleReports;
            if (cycleReports != null)
            {
                for (int i = 0; i < cycleReports.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        int colStartIndex = 0;
                        CycleReport cycleReport = cycleReports[i];
                        if (cycleReport != null)
                        {
                            XSSFRow row = (XSSFRow)sheet.CreateRow(leftRowIndex);
                            setRowHeight(row);
                            leftRowIndex++;
                            cell = (XSSFCell)row.CreateCell(colStartIndex + 0);
                            cell.CellStyle = headerStyle;
                            cell.SetCellValue(cycleReport.cycleName);

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 1);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("信号计算开始时间");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 2);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("信号计算结束时间");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 3);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("信号个数");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 4);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最终权益");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 5);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("夏普比率");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 6);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("权益最大回撤");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 7);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("权益最大回撤比");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 8);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("风险率");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 9);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("每手最大亏损");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 10);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("每手平均盈亏");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 11);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("胜率");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 12);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("模型得分");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 13);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大盈利");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 14);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大亏损");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 15);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大持续盈利次数");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 16);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大持续亏损次数");


                            List<ModelReport> modelReports = cycleReport.modelReports;
                            if (modelReports != null)
                            {
                                foreach (ModelReport modelReport in modelReports)
                                {
                                    row = (XSSFRow)sheet.CreateRow(leftRowIndex);
                                    setRowHeight(row);
                                    leftRowIndex++;
                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 0);
                                    if (modelReport.warning)
                                    {
                                        cell.CellStyle = warningStyle;
                                    }
                                    else
                                    {
                                        cell.CellStyle = commonStyle;
                                    }
                                    cell.SetCellValue(modelReport.modelName);

                                    //期望模型名称和实际模型名称不一致，则不导出数据
                                    bool isErrorData = false;
                                    if (!modelReport.modelName.Equals(modelReport.realisticModelName))
                                    {
                                        isErrorData = true;
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 1);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.startingDate);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 2);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.endingDate);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 3);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.signalNumber);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 4);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.lastInterest);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 5);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.sharpeRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 6);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.interestMaxRetracement);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 7);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.interestMaxRetracementRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 8);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.hazardRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 9);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxLossPerHand);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 10);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.avgProfitLossPerHand);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 11);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.winRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 12);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.score);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 13);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxProfit);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 14);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxLoss);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 15);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxContinuousProfitabilityTimes);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 16);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxContinuousLossesTimes);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int colStartIndex = 18;
                        CycleReport cycleReport = cycleReports[i];
                        if (cycleReport != null)
                        {
                            XSSFRow row = (XSSFRow)sheet.GetRow(rightRowIndex);
                            rightRowIndex++;
                            cell = (XSSFCell)row.CreateCell(colStartIndex + 0);
                            cell.CellStyle = headerStyle;
                            cell.SetCellValue(cycleReport.cycleName);

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 1);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("信号计算开始时间");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 2);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("信号计算结束时间");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 3);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("信号个数");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 4);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最终权益");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 5);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("夏普比率");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 6);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("权益最大回撤");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 7);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("权益最大回撤比");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 8);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("风险率");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 9);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("每手最大亏损");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 10);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("每手平均盈亏");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 11);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("胜率");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 12);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("模型得分");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 13);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大盈利");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 14);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大亏损");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 15);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大持续盈利次数");

                            cell = (XSSFCell)row.CreateCell(colStartIndex + 16);
                            cell.CellStyle = titleStyle;
                            cell.SetCellValue("最大持续亏损次数");


                            List<ModelReport> modelReports = cycleReport.modelReports;
                            if (modelReports != null)
                            {
                                foreach (ModelReport modelReport in modelReports)
                                {
                                    row = (XSSFRow)sheet.GetRow(rightRowIndex);
                                    rightRowIndex++;
                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 0);
                                    if (modelReport.warning)
                                    {
                                        cell.CellStyle = warningStyle;
                                    }
                                    else
                                    {
                                        cell.CellStyle = commonStyle;
                                    }
                                    cell.SetCellValue(modelReport.modelName);

                                    //期望模型名称和实际模型名称不一致，则不导出数据
                                    bool isErrorData = false;
                                    if (!modelReport.modelName.Equals(modelReport.realisticModelName))
                                    {
                                        isErrorData = true;
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 1);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.startingDate);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 2);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.endingDate);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 3);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.signalNumber);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 4);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.lastInterest);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 5);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.sharpeRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 6);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.interestMaxRetracement);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 7);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.interestMaxRetracementRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 8);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.hazardRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 9);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxLossPerHand);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 10);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.avgProfitLossPerHand);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 11);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.winRatio);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 12);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.score);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 13);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxProfit);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 14);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxLoss);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 15);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxContinuousProfitabilityTimes);
                                    }

                                    cell = (XSSFCell)row.CreateCell(colStartIndex + 16);
                                    cell.CellStyle = commonStyle;
                                    if (isErrorData)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(modelReport.maxContinuousLossesTimes);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            String filePath = Utils.getExportDir() + varietyReport.varietyName + "_" + Variety.getAgreementName(varietyReport.agreement) + "_" + Utils.getDate() + "_回测" + "_" + Utils.getTimeMillisecond() + ".xlsx";
            FileStream file = new FileStream(filePath, FileMode.Create);
            workbook.Write(file);
            file.Close();
        }


        private void initStyle(XSSFWorkbook workbook)
        {
            commonStyle = createCommonStyle(workbook);
            titleStyle = createTitleStyle(workbook);
            headerStyle = createHeaderStyle(workbook);
            warningStyle = createWarningStyle(workbook);
        }

        private XSSFCellStyle createCommonStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 9;
            font.FontName = "宋体";
            style.SetFont(font);
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }

        private XSSFCellStyle createWarningStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 9;
            font.FontName = "宋体";
            style.SetFont(font);
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            XSSFColor color = new XSSFColor(new byte[] { 255, 255, 0 });
            style.FillForegroundColorColor = color;
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }

        private XSSFCellStyle createTitleStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 9;
            font.FontName = "宋体";
            font.Boldweight = (short)FontBoldWeight.Bold;
            style.SetFont(font);
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }

        private XSSFCellStyle createHeaderStyle(XSSFWorkbook workbook)
        {
            XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = false;
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 9;
            font.FontName = "宋体";
            font.Boldweight = (short)FontBoldWeight.Bold;
            XSSFColor color = new XSSFColor(new byte[] { 255, 0, 0 });
            font.SetColor(color);
            style.SetFont(font);
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }

        private void setColumnWidth(XSSFSheet sheet)
        {
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 14 * 256);
            sheet.SetColumnWidth(2, 14 * 256);
            sheet.SetColumnWidth(3, 10 * 256);
            sheet.SetColumnWidth(4, 10 * 256);
            sheet.SetColumnWidth(5, 10 * 256);
            sheet.SetColumnWidth(6, 11 * 256);
            sheet.SetColumnWidth(7, 13 * 256);
            sheet.SetColumnWidth(8, 6 * 256);
            sheet.SetColumnWidth(9, 11 * 256);
            sheet.SetColumnWidth(10, 11 * 256);
            sheet.SetColumnWidth(11, 7 * 256);
            sheet.SetColumnWidth(12, 7 * 256);
            sheet.SetColumnWidth(13, 10 * 256);
            sheet.SetColumnWidth(14, 10 * 256);
            sheet.SetColumnWidth(15, 15 * 256);
            sheet.SetColumnWidth(16, 15 * 256);
            sheet.SetColumnWidth(17, 6 * 256);

            sheet.SetColumnWidth(18, 10 * 256);
            sheet.SetColumnWidth(19, 14 * 256);
            sheet.SetColumnWidth(20, 14 * 256);
            sheet.SetColumnWidth(21, 10 * 256);
            sheet.SetColumnWidth(22, 10 * 256);
            sheet.SetColumnWidth(23, 10 * 256);
            sheet.SetColumnWidth(24, 11 * 256);
            sheet.SetColumnWidth(25, 13 * 256);
            sheet.SetColumnWidth(26, 6 * 256);
            sheet.SetColumnWidth(27, 11 * 256);
            sheet.SetColumnWidth(28, 11 * 256);
            sheet.SetColumnWidth(29, 7 * 256);
            sheet.SetColumnWidth(30, 7 * 256);
            sheet.SetColumnWidth(31, 10 * 256);
            sheet.SetColumnWidth(32, 10 * 256);
            sheet.SetColumnWidth(33, 15 * 256);
            sheet.SetColumnWidth(34, 15 * 256);
        }

        private void setRowHeight(XSSFRow row)
        {
            row.Height = 270;
        }
    }
}
